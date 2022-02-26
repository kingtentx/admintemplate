using King.AdminSite.Models;
using King.Data;
using King.Helper;
using King.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace King.AdminSite.Controllers
{
    [Authorize]
    public class RoleController : AdminBaseController
    {

        private IBllService<Role> _roleService;
        private IBllService<RoleMenu> _roleMenuService;
        private IPermission _permission;
        private ICacheService _cache;

        public RoleController(ICacheService cache, IBllService<Role> roleService, IBllService<RoleMenu> roleMenuService, IPermission permission)
        {
            _roleService = roleService;
            _roleMenuService = roleMenuService;
            _cache = cache;
            _permission = permission;

        }

        public IActionResult Index()
        {
            if (!_permission.CheckPermission(LoginUser, MenuCode.System_Role, PermissionType.View))
            {
                return Content("无访问权限");
            }

            ViewData[PageCode.PAGE_Button_Add] = _permission.CheckPermission(LoginUser, MenuCode.System_Role, PermissionType.Add);
            ViewData[PageCode.PAGE_Button_Edit] = _permission.CheckPermission(LoginUser, MenuCode.System_Role, PermissionType.Edit);
            ViewData[PageCode.PAGE_Button_Delete] = _permission.CheckPermission(LoginUser, MenuCode.System_Role, PermissionType.Delete);
            ViewData[PageCode.PAGE_Button_Authorize] = _permission.CheckPermission(LoginUser, MenuCode.System_Role, PermissionType.Authorize);

            return View();
        }

        public ActionResult GetList(int pageIndex = 1, int pageSize = 10)
        {
            AjaxResultList result = new AjaxResultList();

            if (!_permission.CheckPermission(LoginUser, MenuCode.System_Role, PermissionType.View))
            {
                result.Code = (int)ResultCode.Nopermit;
                result.Msg = "无操作权限";

                return Json(result);
            }

            var rolename = HttpContext.Request.Query["rolename"].ToString() ?? "";

            var where = LambdaHelper.True<Role>();

            if (!string.IsNullOrWhiteSpace(rolename))
                where = where.And(p => p.RoleName == rolename);

            var query = _roleService.GetList(where, p => p.RoleId, pageIndex, pageSize);

            var data = from q in query.List
                       select new RoleModel()
                       {
                           RoleID = q.RoleId,
                           RoleName = q.RoleName,
                           Description = q.Description,
                           IsActive = q.IsActive
                       };

            result.Code = (int)ResultCode.Success;
            result.Msg = "成攻";
            result.Count = query.Count;
            result.Data = data;

            return Json(result);
        }


        public ActionResult Edit(int? id)
        {
            if (!_permission.CheckPermission(LoginUser, MenuCode.System_Role, PermissionType.View))
            {
                return Content("无访问权限");
            }

            if (id.HasValue == false) id = 0;

            RoleModel model = new RoleModel();
            if (id > 0)
            {
                var role = _roleService.GetOne(id.Value);
                if (role == null)
                    return View();

                model.RoleID = role.RoleId;
                model.RoleName = role.RoleName;
                model.IsActive = role.IsActive;
                model.Description = role.Description;

            }

            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        public ActionResult EditPost(int? id, RoleModel input)
        {
            AjaxResult result = new AjaxResult() { Code = (int)ResultCode.ParmsError, Msg = "失败" };

            if (id.HasValue == false) id = 0;

            if (id > 0)
            {
                if (!_permission.CheckPermission(LoginUser, MenuCode.System_Role, PermissionType.Edit))
                {
                    result.Code = (int)ResultCode.Nopermit;
                    result.Msg = "无操作权限";
                    return Json(result);
                }

                var editmodel = _roleService.GetOne(id.Value);
                editmodel.RoleId = id.Value;
                editmodel.RoleName = input.RoleName;

                editmodel.IsActive = input.IsActive;
                editmodel.Description = input.Description;

                if (_roleService.Update(editmodel))
                {
                    result.Code = (int)ResultCode.Success;
                    result.Msg = "修改成功";
                }

            }
            else
            {
                if (!_permission.CheckPermission(LoginUser, MenuCode.System_Role, PermissionType.Add))
                {
                    result.Code = (int)ResultCode.Nopermit;
                    result.Msg = "无操作权限";
                    return Json(result);
                }

                Role model = new Role();
                model.RoleName = input.RoleName;
                model.IsActive = input.IsActive;
                model.Description = input.Description;
                model.CreateTime = DateTime.Now;

                if (_roleService.Add(model).RoleId > 0)
                {
                    result.Code = (int)ResultCode.Success;
                    result.Msg = "添加成功";
                }
            }

            return Json(result);
        }

        [HttpPost]
        public ActionResult Delete(int? id)
        {
            AjaxResult result = new AjaxResult() { Code = (int)ResultCode.ParmsError, Msg = "失败" };

            if (!_permission.CheckPermission(LoginUser, MenuCode.System_Role, PermissionType.Delete))
            {
                result.Code = (int)ResultCode.Nopermit;
                result.Msg = "无操作权限";

                return Json(result);
            }

            var i = _roleService.Delete(id.Value);
            if (i)
            {
                result.Code = (int)ResultCode.Success;
                result.Msg = "删除成功！";
            }

            return Json(result);
        }



        public ActionResult Authorize(int? id)
        {
            if (!_permission.CheckPermission(LoginUser, MenuCode.System_Role, PermissionType.Authorize))
            {
                BadRequest("无操作权限");
                return Redirect("/Home/ReLogin");
            }

            if (id.HasValue == false) id = 0;

            RoleModel model = new RoleModel();
            model.MenuList = _permission.GetRoleMenus(LoginUser);

            if (id > 0)
            {
                var role = _roleService.GetOne(id.Value);
                if (role == null)
                    return View();

                model.RoleID = role.RoleId;
                model.RoleName = role.RoleName;
                model.IsActive = role.IsActive;
                model.Description = role.Description;

                var rolemenu = _roleMenuService.GetList(p => p.RoleID == id);
                if (rolemenu.Count > 0)
                {
                    model.PermissionList = rolemenu.Select(p => p.Permission).ToArray();
                }
            }

            return View(model);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="Menus"></param>
        /// <param name="roleid"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveRoleMenu(string[] Menus, int roleid = 0)
        {
            AjaxResult result = new AjaxResult() { Code = (int)ResultCode.ParmsError, Msg = "失败" };

            if (!_permission.CheckPermission(LoginUser, MenuCode.System_Role, PermissionType.Authorize))
            {
                result.Code = (int)ResultCode.Nopermit;
                result.Msg = "无操作权限";
                return Json(result);
            }

            if (Menus.Length == 0)
            {
                result.Code = (int)ResultCode.ParmsError;
                result.Msg = "请选择菜单";
            }
            else
            {

                var oldArray = _roleMenuService.GetList(p => p.RoleID == roleid).Select(p => p.Permission).ToArray();
                var newArray = Menus;

                var delArray = oldArray.Except(newArray).ToArray();//新菜单中没有的删除
                var addArray = newArray.Except(oldArray).ToArray();//旧菜单中没有的新增

                //删除
                if (delArray.Length > 0)
                {
                    if (!string.IsNullOrEmpty(delArray[0]))
                    {
                        _roleMenuService.Delete(p => p.RoleID == roleid && delArray.Contains(p.Permission));
                    }
                }
                //新增
                if (addArray.Length > 0)
                {
                    if (!string.IsNullOrEmpty(addArray[0]))
                    {
                        List<RoleMenu> list = new List<RoleMenu>();
                        foreach (var item in addArray)
                        {
                            RoleMenu model = new RoleMenu();
                            model.RoleID = roleid;
                            model.CreateTime = DateTime.Now;
                            model.Permission = item;
                            list.Add(model);
                        }
                        _roleMenuService.Add(list);
                    }
                }

                //修改角色菜单后清空前角色缓存
                _cache.Remove(CacheKey.Permission_Menu + roleid);

                result.Code = (int)ResultCode.Success;
                result.Msg = "保存成功";

            }
            return Json(result);
        }

    }
}