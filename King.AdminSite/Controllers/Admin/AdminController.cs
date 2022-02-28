using AutoMapper;
using King.AdminSite.Models;
using King.Data;
using King.Helper;
using King.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace King.AdminSite.Controllers
{
    [Authorize]
    public class AdminController : AdminBaseController
    {

        private ICacheService _cache;
        private IMapper _mapper;
        private IPermission _permission;
        private IBllService<Admin> _adminService;
        private IBllService<SiteConfig> _siteService;



        public AdminController(
             ICacheService cache,
             IMapper mapper,
             IPermission permission,
             IBllService<Admin> adminService,
             IBllService<SiteConfig> siteService
            )
        {

            _cache = cache;
            _mapper = mapper;
            _adminService = adminService;
            _permission = permission;
            _siteService = siteService;

        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult ReLogin()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            //清空前角色缓存
            _cache.Remove(CacheKey.Permission_Menu + LoginUser.Roles);

            return Json(new AjaxResult() { Code = (int)ResultCode.Success, Msg = "ok" });

        }

        public IActionResult Index()
        {
            var user = LoginUser;
            ViewBag.UserName = user.UserName;
            var model = _permission.GetLeftMenus(user);
            return View(model);
        }

        public ActionResult Main()
        {
            //var data = NavigateData.DataList();
            ViewBag.Data = "";

            return View();
        }

        public ActionResult Password()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UpdatePassword(string txtOld, string txtNew, string txtNew2)
        {
            AjaxResult result = new AjaxResult() { Code = (int)ResultCode.ParmsError, Msg = "参数不全" };

            if (string.IsNullOrEmpty(txtNew))
            {
                result.Msg = "请输入新密码！";
                return Json(result);

            }
            if (txtNew != txtNew2)
            {
                result.Msg = "两次密码输入不相同!";
                return Json(result);
            }


            var admin = _adminService.GetOne(LoginUser.UserId);

            if (admin != null && admin.Password == StringHelper.ToMD5(txtOld))
            {
                admin.Password = StringHelper.ToMD5(txtNew);
                if (_adminService.Update(admin))
                {
                    result.Code = (int)ResultCode.Success;
                    result.Msg = "修改成功";
                }
            }
            else
            {
                result.Code = (int)ResultCode.Fail;
                result.Msg = "原密码输入错误！";
            }

            return Json(result);
        }

        public async Task<IActionResult> SiteInfo()
        {
            var model = new SiteConfigModel();
            var query = await _siteService.GetOneAsync(p => p.IsActive == true);

            if (query != null)
                model = _mapper.Map<SiteConfigModel>(query);

            return View(model);
        }

        [HttpPost, ActionName("SiteInfo")]
        public async Task<IActionResult> SiteInfoEdit(SiteConfigModel input)
        {
            AjaxResult result = new AjaxResult() { Code = (int)ResultCode.Fail, Msg = "失败" };

            //if (!_permission.CheckPermission(LoginUser, MenuCode.System_Admin, PermissionType.Edit))
            //{
            //    result.Code = (int)ResultCode.Nopermit;
            //    result.Msg = "无操作权限";

            //    return Json(result);
            //}

            var model = _mapper.Map<SiteConfig>(input);

            var query = await _siteService.GetModel(p => p.IsActive == true).AsNoTracking().FirstOrDefaultAsync();
            if (query != null)
            {
                model.Id = query.Id;
                model.CreateTime = query.CreateTime;
                model.CreateBy = query.CreateBy;
                model.UpdateTime = DateTime.Now;
                model.UpdateBy = LoginUser.UserName;

                if (await _siteService.UpdateAsync(model))
                {
                    result.Code = ResultCode.Success.GetHashCode();
                    result.Msg = "保存成功";
                }
            }
            else
            {
                model.CreateTime = DateTime.Now;
                model.CreateBy = LoginUser.UserName;

                await _siteService.AddAsync(model);
                if ((await _siteService.AddAsync(model)).Id > 0)
                {
                    result.Code = ResultCode.Success.GetHashCode();
                    result.Msg = "保存成功";
                }
            }
            return Json(result);
        }
    }
}
