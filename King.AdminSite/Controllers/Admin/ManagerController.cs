using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using King.AdminSite.Models;
using King.Data;
using King.Helper;
using King.Interface;
using King.Utils.Enums;

namespace King.AdminSite.Controllers
{
    [Authorize]
    public class ManagerController : AdminBaseController
    {       
        private IPermission _permission;
        private IBllService<Admin> _adminService;
        private IBllService<Role> _roleService;
        private IBllService<AdminLogin> _loginService;


        public ManagerController(IPermission permission, IBllService<Admin> adminService, IBllService<Role> roleService, IBllService<AdminLogin> loginService)
        {
            _adminService = adminService;
            _roleService = roleService;
            _permission = permission;
            _loginService = loginService;

        }

        public IActionResult Index()
        {

            if (!_permission.CheckPermission(LoginUser, MenuCode.System_Admin, PermissionType.View))
            {
                return Content("无访问权限");
            }  

            ViewData[PageCode.PAGE_Button_Add] = _permission.CheckPermission(LoginUser, MenuCode.System_Admin, PermissionType.Add);
            ViewData[PageCode.PAGE_Button_Edit] = _permission.CheckPermission(LoginUser, MenuCode.System_Admin, PermissionType.Edit);
            ViewData[PageCode.PAGE_Button_Delete] = _permission.CheckPermission(LoginUser, MenuCode.System_Admin, PermissionType.Delete);

            return View();
        }

        [HttpGet]
        public JsonResult GetList(int pageIndex = 1, int pageSize = 10)
        {

            AjaxResultList result = new AjaxResultList() { Code = (int)ResultCode.ParmsError, Msg = "失败" };

            if (!_permission.CheckPermission(LoginUser, MenuCode.System_Admin, PermissionType.View))
            {
                result.Code = (int)ResultCode.Nopermit;
                result.Msg = "无操作权限";

                return Json(result);
            }

            var username = HttpContext.Request.Query["username"];

            var where = LambdaHelper.True<Admin>().And(p => p.IsAdmin == false);

            if (!string.IsNullOrWhiteSpace(username))
                where = where.And(p => p.UserName == username);

            var query = _adminService.GetList(where, p => p.AdminId, pageIndex, pageSize);

            var data = from q in query.List
                       select new AdminModel()
                       {
                           AdminID = q.AdminId,
                           UserName = q.UserName,
                           RealName = q.RealName,
                           CreateTime = q.CreateTime,
                           IsAdmin = q.IsAdmin,
                           IsActive = q.IsActive,
                           Roles = !string.IsNullOrEmpty(q.Roles) ? q.Roles.Split(',') : new string[] { }
                       };

            result.Code = (int)ResultCode.Success;
            result.Msg = "成攻";
            result.Count = query.Count;
            result.Data = data;

            return Json(result);
        }

        public ActionResult Edit(int? id)
        {

            if (!_permission.CheckPermission(LoginUser, MenuCode.System_Admin, PermissionType.View))
            {
                return Content("无访问权限");
            }

            if (id.HasValue == false) id = 0;

            AdminModel model = new AdminModel();
            if (id > 0)
            {
                var admin = _adminService.GetOne(id.Value);
                if (admin == null)
                    return View();

                model.AdminID = admin.AdminId;
                model.UserName = admin.UserName;
                model.RealName = admin.RealName;
                model.IsAdmin = admin.IsAdmin;
                model.IsActive = admin.IsActive;
                model.Remark = admin.Remark;
                model.Roles = !string.IsNullOrWhiteSpace(admin.Roles) ? admin.Roles.Split(',') : new string[] { };

            }
            var roledata = _roleService.GetList(p => p.IsActive == true);
            var rolellist = from r in roledata
                            select new RoleModel()
                            {
                                RoleID = r.RoleId,
                                RoleName = r.RoleName
                            };
            model.RoleList = rolellist.ToList();

            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        public ActionResult EditPost(int? id, AdminModel input)
        {

            AjaxResult result = new AjaxResult() { Code = (int)ResultCode.ParmsError, Msg = "失败" };

            if (id.HasValue == false) id = 0;

            if (id > 0)
            {
                if (!_permission.CheckPermission(LoginUser, MenuCode.System_Admin, PermissionType.Edit))
                {
                    result.Code = (int)ResultCode.Nopermit;
                    result.Msg = "无操作权限";

                    return Json(result);
                }

                var editmodel = _adminService.GetOne(id.Value);

                //是否修改系统超级管理员信息
                var user = LoginUser;
                if (!user.IsAdmin)
                {
                    if (editmodel.IsAdmin)
                    {
                        result.Code = (int)ResultCode.Nopermit;
                        result.Msg = "无操作权限";
                        return Json(result);
                    }
                }
                editmodel.AdminId = id.Value;
                editmodel.RealName = input.RealName;
                editmodel.IsAdmin = false;
                editmodel.IsActive = input.IsActive;
                editmodel.Remark = input.Remark;
                editmodel.Roles = string.Join(",", input.Roles);
                editmodel.UpdateTime = DateTime.Now;

                if (!string.IsNullOrEmpty(input.Password))
                    editmodel.Password = StringHelper.ToMD5(input.Password);

                if (_adminService.Update(editmodel))
                {
                    result.Code = (int)ResultCode.Success;
                    result.Msg = "修改成功";
                }

            }
            else
            {
                if (!_permission.CheckPermission(LoginUser, MenuCode.System_Admin, PermissionType.Add))
                {
                    result.Code = (int)ResultCode.Nopermit;
                    result.Msg = "无操作权限";

                    return Json(result);
                }

                var admin = _adminService.GetOne(p => p.UserName.ToLower() == input.UserName.ToLower());
                if (admin != null)
                {
                    result.Code = (int)ResultCode.Fail;
                    result.Msg = "用户名已存在";
                }
                else
                {
                    Admin model = new Admin();
                    model.UserName = input.UserName;
                    model.RealName = input.RealName;
                    model.IsAdmin = false;
                    model.IsActive = input.IsActive;
                    model.Remark = input.Remark;
                    model.Roles = string.Join(",", input.Roles);
                    model.Password = StringHelper.ToMD5(input.Password);
                    model.CreateTime = DateTime.Now;

                    if (_adminService.Add(model).AdminId > 0)
                    {
                        result.Code = (int)ResultCode.Success;
                        result.Msg = "添加成功";
                    }
                }
            }

            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {
            AjaxResult result = new AjaxResult() { Code = (int)ResultCode.ParmsError, Msg = "失败" };

            if (!_permission.CheckPermission(LoginUser, MenuCode.System_Admin, PermissionType.Delete))
            {
                result.Code = (int)ResultCode.Nopermit;
                result.Msg = "无操作权限";

                return Json(result);
            }

            try
            {
                //是否删除超级管理员
                if (_adminService.GetOne(id).IsAdmin)
                {
                    result.Code = (int)ResultCode.Nopermit;
                    result.Msg = "无操作权限";
                    return Json(result);
                }

                //int[] intArray = Array.ConvertAll<string, int>(ids, s => int.Parse(s));
                var i = await _adminService.DeleteAsync(id);
                if (i)
                {
                    result.Code = (int)ResultCode.Success;
                    result.Msg = "删除成功！";
                    log.Info($"用户：{LoginUser.UserName}操作-[删除管理员]：{id}");
                }
            }
            catch (Exception ex)
            {
                result.Code = (int)ResultCode.Fail;
                result.Msg = "删除失败";
                log.Error($"用户：{LoginUser.UserName}操作-[删除管理员异常]：" + ex.Message);
            }
            return Json(result);
        }

        public IActionResult LoginInfo()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> LoginList(int pageIndex = 1, int pageSize = 10)
        {
            AjaxResultList result = new AjaxResultList() { Code = (int)ResultCode.ParmsError, Msg = "失败" };

            if (!_permission.CheckPermission(LoginUser, MenuCode.System_Admin, PermissionType.View))
            {
                result.Code = (int)ResultCode.Nopermit;
                result.Msg = "无操作权限";
                return Json(result);
            }

            var username = HttpContext.Request.Query["username"];

            var where = LambdaHelper.True<AdminLogin>();

            if (!string.IsNullOrWhiteSpace(username))
                where = where.And(p => p.UserName == username);

            var query = await _loginService.GetListAsync(where, p => p.Id, pageIndex, pageSize);

            result.Code = (int)ResultCode.Success;
            result.Msg = "成攻";
            result.Count = query.Count;
            result.Data = query.List;

            return Json(result);
        }
    }
}