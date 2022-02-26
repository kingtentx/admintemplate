using log4net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using King.AdminSite.Config;
using King.AdminSite.Models;
using King.AdminSite.WeCat;
using King.Data;
using King.Helper;
using King.Interface;
using King.Utils;
using King.Wecat;
using King.Wecat.Models;

namespace King.AdminSite.Controllers
{
    public class AuthorizeController : Controller
    {
        private ILog log = LogManager.GetLogger(Startup.logRepository.Name, typeof(AuthorizeController));
        private ICacheService _cache;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IBllService<Admin> _adminService;
        private IBllService<AdminLogin> _loginService;
        private WecatConfig _wecatConfig;
        private ResponseMessage _rpMsg;

        public AuthorizeController(ICacheService cache,
             IHttpContextAccessor httpContextAccessor,
             IOptions<WecatConfig> wecat, ResponseMessage rpMsg, IBllService<Admin> adminService, IBllService<AdminLogin> loginService)
        {
            _adminService = adminService;
            _loginService = loginService;
            _cache = cache;
            _wecatConfig = wecat.Value;
            _rpMsg = rpMsg;
        }

        #region 登录
        /// <summary>
        /// asp.net cookies 登录
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Login")]
        public async Task<IActionResult> LoginPost(LoginViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var code = _cache.Get(CacheKey.ValidateCode + viewModel.ValidateKey) ?? "";
                if (viewModel.ValidateCode.ToLower() != code.ToString().ToLower() || string.IsNullOrEmpty(viewModel.ValidateCode))
                {
                    return BadRequest("验证码错误");
                }
                if (!string.IsNullOrEmpty(viewModel.UserName) && !string.IsNullOrEmpty(viewModel.Password))//判断账号密码是否正确
                {
                    var admin = _adminService.GetOne(p => p.UserName == viewModel.UserName && p.Password == StringHelper.ToMD5(viewModel.Password));
                    if (admin == null)
                        return BadRequest("用户名密码错误");

                    if (!admin.IsActive)
                        return BadRequest("帐户已被禁用");

                    //创建用户身份标识
                    var claimsIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                    claimsIdentity.AddClaims(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Sid,admin.AdminId.ToString()),
                        new Claim(ClaimTypes.Name,admin.UserName),
                        new Claim(ClaimTypes.Role,!string.IsNullOrWhiteSpace(admin.Roles)?admin.Roles:"0"),
                        new Claim(ClaimTypes.System,admin.IsAdmin.ToString())
                    });

                    //登录用户，相当于ASP.NET中的FormsAuthentication.SetAuthCookie
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity)
                        , new AuthenticationProperties
                        {
                            ExpiresUtc = DateTime.UtcNow.AddMinutes(CacheKey.Expiration_Min_60),
                            // 持久保存
                            IsPersistent = true,
                            //AllowRefresh为true，表示如果用户登录后在超过50%的ExpiresUtc时间间隔内又访问了站点，就延长用户的登录时间
                            AllowRefresh = true
                        });

                    HttpContextAccessor context = new HttpContextAccessor();
                    var ip = context.HttpContext?.Connection.RemoteIpAddress.ToString();
                    //if (_httpContextAccessor.HttpContext != null)
                    // string ip = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

                    var model = new AdminLogin()
                    {
                        UserName = admin.UserName,
                        Client = Request.Headers["User-Agent"].ToString(),
                        LoginDate = DateTime.Now,
                        LoginIp = ip
                    };

                    await _loginService.AddAsync(model);

                    return Json(new AjaxResult() { Code = (int)ResultCode.Success, Msg = "ok" });
                }
            }

            return BadRequest("帐号或者密码错误");
        }

        /// <summary>
        /// 验证码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetImg(string key, int width = 130, int height = 38, int fontsize = 24)
        {
            string code = string.Empty;
            byte[] bytes = ValidateCode.CreateValidateGraphic(out code, 4, width, height, fontsize);

            _cache.Add(CacheKey.ValidateCode + key, code, TimeSpan.FromMinutes(5));
            return File(bytes, @"image/jpeg");
        }
        #endregion

        #region 微信验签

        [HttpGet]
        public IActionResult Token(PostModel postModel, string echostr)
        {

            if (WeixinHelper.CheckSignature(postModel.Signature, postModel.Timestamp, postModel.Nonce, _wecatConfig.Token))
            {
                log.Info($"绑定成功:Signature:{postModel.Signature},AppId:{_wecatConfig.AppId},token:{_wecatConfig.Token}");
                return Content(echostr); //返回随机字符串则表示验证通过
            }
            else
            {
                return Content("failed:" + postModel.Signature + "," + "如果你在浏览器中看到这句话，说明此地址可以被作为微信公众账号后台的Url，请注意保持Token一致。");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Token(PostModel postModel)
        {
            log.Info("host:" + Request.Scheme + "://" + Request.Host.Value);

            var openid = HttpContext.Request.Query["openid"].ToString();
            if (string.IsNullOrWhiteSpace(openid))
            {
                return Content("非微信请求");
            }
            //log.Info("openid:" + openid);         

            try
            {
                if (!WeixinHelper.CheckSignature(postModel.Signature, postModel.Timestamp, postModel.Nonce, _wecatConfig.Token))
                {
                    return Content("参数错误！");
                }
                using (Stream stream = HttpContext.Request.Body)
                {
                    byte[] buffer = new byte[HttpContext.Request.ContentLength.Value];
                    stream.Read(buffer, 0, buffer.Length);
                    string content = System.Text.Encoding.UTF8.GetString(buffer);

                    if (!string.IsNullOrWhiteSpace(postModel.Msg_Signature)) // 消息加密模式
                    {
                        string decryptMsg = string.Empty;
                        WXBizMsgCrypt wxBizMsgCrypt = new WXBizMsgCrypt(_wecatConfig.Token, _wecatConfig.EncodingAESKey, _wecatConfig.AppId);

                        int decryptResult = wxBizMsgCrypt.DecryptMsg(postModel.Msg_Signature, postModel.Timestamp, postModel.Nonce, content, ref decryptMsg);
                        if (decryptResult == 0 && !string.IsNullOrWhiteSpace(decryptMsg))
                        {
                            var resultMsg = await _rpMsg.Message(decryptMsg);

                            string sEncryptMsg = string.Empty;
                            if (!string.IsNullOrWhiteSpace(resultMsg))
                            {
                                int encryptResult = wxBizMsgCrypt.EncryptMsg(resultMsg, postModel.Timestamp, postModel.Nonce, ref sEncryptMsg);
                                if (encryptResult == 0 && !string.IsNullOrWhiteSpace(sEncryptMsg))
                                {
                                    return Content(sEncryptMsg);
                                }
                            }
                        }
                    }
                    else //消息未加密码处理
                    {
                        return Content(await _rpMsg.Message(content));
                    }
                    return Content(null);
                }
            }
            catch (Exception ex)
            {
                log.Error("接收消息并处理和返回相应结果异常：", ex);
                return Content(null);
            }
        }

        #endregion
    }

}