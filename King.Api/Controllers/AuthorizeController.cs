using King.Api.Models;
using King.Data;
using King.Helper;
using King.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace King.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthorizeController : ControllerBase
    {
        private JwtConfig _jwtSettings;
        private ICacheService _cache;
        private IBllService<User> _userService;



        /// <summary>
        /// 
        /// </summary>
        /// <param name="jwt"></param>
        /// <param name="cache"></param>
        /// <param name="userService"></param>
        public AuthorizeController(IOptions<JwtConfig> jwt, ICacheService cache, IBllService<User> userService)
        {
            _jwtSettings = jwt.Value;
            _cache = cache;
            _userService = userService;
        }


        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Login([FromBody] UserInput model)
        {
            if (ModelState.IsValid)//判断是否合法
            {
                var code = _cache.Get(Common.CACHE_ValidateKey + model.ValidateKey) ?? "";
                if (model.ValidateValue.ToLower() != code.ToString().ToLower() || string.IsNullOrEmpty(model.ValidateValue))
                {
                    return BadRequest("验证码错误");
                }
                if (!string.IsNullOrEmpty(model.Telphone) && !string.IsNullOrEmpty(model.Password))//判断账号密码是否正确
                {
                    var user = await _userService.GetOneAsync(p => p.Telphone == model.Telphone && p.Password == StringHelper.ToMD5(model.Password));
                    if (user == null)
                        return BadRequest("用户名密码错误");


                    var claim = new Claim[]{
                    new Claim(ClaimTypes.Sid,user.Id.ToString()),
                    new Claim(ClaimTypes.Name,user.Telphone)                 
                };

                    //对称秘钥
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
                    //签名证书(秘钥，加密算法)
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    //生成token  [注意]需要nuget添加Microsoft.AspNetCore.Authentication.JwtBearer包，并引用System.IdentityModel.Tokens.Jwt命名空间
                    var token = new JwtSecurityToken(_jwtSettings.Issuer, _jwtSettings.Audience, claim, DateTime.Now, DateTime.Now.AddMinutes(_jwtSettings.Expiration), creds);

                    return Ok(new { UserName = user.Telphone, Token = new JwtSecurityTokenHandler().WriteToken(token) });
                }
            }

            return BadRequest("参数错误");
        }

        /// <summary>
        /// 验证码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetImgBase64()
        {
            var key = DateTime.Now.Ticks.ToString();

            int width = 130; int height = 38; int fontsize = 24;
            string code = string.Empty;
            byte[] bytes = ValidateCode.CreateValidateGraphic(out code, 4, width, height, fontsize);

            var strHeader = "data:image/png;base64,";
            string strBase64 = Convert.ToBase64String(bytes);

            //添加缓存值
            _cache.Add(Common.CACHE_ValidateKey + key, code, TimeSpan.FromMinutes(5));

            var data = strHeader + strBase64;

            return Ok(new { key = key, data = data });
        }

      
    }
}