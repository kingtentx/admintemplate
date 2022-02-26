using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using King.AdminSite.Models;
using King.Data;
using King.Helper;
using King.Interface;
using King.Utils.Enums;
using AutoMapper;
using System.Threading.Tasks;

namespace King.AdminSite.Controllers
{
    [Authorize]
    public class NavigationController : AdminBaseController
    {
        private IPermission _permission;
        private IMapper _mapper;
        private IBllService<Navigation> _navigationService;
        public NavigationController(IMapper mapper, IPermission permission, IBllService<Navigation> categoryService)
        {
            _mapper = mapper;
            _permission = permission;
            _navigationService = categoryService;

        }

        public IActionResult Index()
        {

            //if (!_permission.CheckPermission(LoginUser, MenuCode.Management_Navigation, PermissionType.View))
            //{
            //    return Content("无访问权限");
            //}

       
            //ViewData[PageCode.PAGE_Button_Add] = _permission.CheckPermission(LoginUser, MenuCode.Management_Navigation, PermissionType.Add);
            //ViewData[PageCode.PAGE_Button_Edit] = _permission.CheckPermission(LoginUser, MenuCode.Management_Navigation, PermissionType.Edit);
            //ViewData[PageCode.PAGE_Button_Delete] = _permission.CheckPermission(LoginUser, MenuCode.Management_Navigation, PermissionType.Delete);

            return View();
        }

        public JsonResult GetList()
        {

            AjaxResultList result = new AjaxResultList();

            //if (!_permission.CheckPermission(LoginUser, MenuCode.Management_Navigation, PermissionType.View))
            //{
            //    result.Code = (int)ResultCode.Nopermit;
            //    result.Msg = "无操作权限";

            //    return Json(result);
            //}

            var query = _navigationService.GetList(p => p.IsDelete == false).OrderBy(p => p.Sort);
          

            var data = _mapper.Map<List<NavigationModel>>(query);

            result.Code = (int)ResultCode.Success;
            result.Msg = "成攻";
            result.Count = data.Count();
            result.Data = data;

            return Json(result);
        }

        public ActionResult Edit(int id = 0, int pid = 0)
        {
            //if (!_permission.CheckPermission(LoginUser, MenuCode.Management_Navigation, PermissionType.View))
            //{
            //    return Content("无访问权限");
            //}

            NavigationModel model = new NavigationModel();

            if (id == 0 && pid == 0)//新增菜单
            {
                ViewData["Pid"] = pid;
            }
            else if (id == 0 && pid > 0) //添加子类
            {
                ViewData["Pid"] = pid;
            }
            else //编辑
            {              
                var query = _navigationService.GetOne(id);
                model = _mapper.Map<NavigationModel>(query);
                ViewData["Pid"] = model.Pid;
            }

            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        public async Task<ActionResult> EditPost(int id, NavigationModel input)
        {
            AjaxResult result = new AjaxResult() { Code = (int)ResultCode.ParmsError, Msg = "失败" };

            if (id > 0)
            {
                //if (!_permission.CheckPermission(LoginUser, MenuCode.Management_Navigation, PermissionType.Edit))
                //{
                //    result.Code = (int)ResultCode.Nopermit;
                //    result.Msg = "无操作权限";
                //    return Json(result);
                //}           

                var editmodel =await _navigationService.GetOneAsync(id);

                editmodel.NavigationName = input.NavigationName;
                editmodel.ParentId = input.Pid;            
                editmodel.RewriteName = input.RewriteName;
                editmodel.Description = input.Description;
                editmodel.IsActive = input.IsActive;
                editmodel.IsShow = input.IsShow;
                editmodel.Sort = input.Sort;
                editmodel.LinkUrl = input.LinkUrl;
                editmodel.UpdateBy = LoginUser.UserName;
                editmodel.UpdateTime = DateTime.Now;

                if (await _navigationService.UpdateAsync(editmodel))
                {
                    result.Code = (int)ResultCode.Success;
                    result.Msg = "修改成功";
                }
            }
            else
            {
                //if (!_permission.CheckPermission(LoginUser, MenuCode.Management_Navigation, PermissionType.Add))
                //{
                //    result.Code = (int)ResultCode.Nopermit;
                //    result.Msg = "无操作权限";
                //    return Json(result);
                //}

                Navigation model = new Navigation();
                model.NavigationName = input.NavigationName;
                model.ParentId = input.Pid;              
                model.RewriteName = input.RewriteName;
                model.Description = input.Description;
                model.IsActive = input.IsActive;
                model.IsShow = input.IsShow;
                model.Sort = input.Sort;
                model.LinkUrl = input.LinkUrl;
                model.CreateBy = LoginUser.UserName;
                model.CreateTime = DateTime.Now;

                if (_navigationService.Add(model).NavigationId > 0)
                {
                    result.Code = (int)ResultCode.Success;
                    result.Msg = "添加成功";
                }
            }

            return Json(result);
        }

        public JsonResult GetData()
        {

            //if (!_permission.CheckPermission(LoginUser, MenuCode.Management_Navigation, PermissionType.View))
            //{
            //    AjaxResult result = new AjaxResult();
            //    result.Code = (int)ResultCode.Nopermit;
            //    result.Msg = "无操作权限";
            //    return Json(result);
            //}

            var list = _navigationService.GetList(p => p.IsActive == true && p.IsDelete == false);

            List<TreeModel> treeList = new List<TreeModel>();
            foreach (var parentNodeList in list.Where(t => t.ParentId == 0))
            {
                TreeModel model = new TreeModel();
                model.Id = parentNodeList.NavigationId;
                model.Name = parentNodeList.NavigationName;
                model.Sort = parentNodeList.Sort;

                model.Children = PageUtils.NavigationTree(list, model);
                treeList.Add(model);
            }
            return Json(treeList.OrderBy(p => p.Sort));
        }

        [HttpPost]
        public JsonResult Delete(int id = 0)
        {

            AjaxResult result = new AjaxResult() { Code = (int)ResultCode.ParmsError, Msg = "参数不全" };

            //if (!_permission.CheckPermission(LoginUser, MenuCode.Management_Navigation, PermissionType.Delete))
            //{
            //    result.Code = (int)ResultCode.Nopermit;
            //    result.Msg = "无操作权限";

            //    return Json(result);
            //}

            if (id > 0)
            {
                var list = _navigationService.GetList(p => p.ParentId == id && p.IsDelete == false);
                if (list.Count() > 0)
                {
                    result.Code = (int)ResultCode.Fail;
                    result.Msg = "请先删除全部子菜单";
                }
                else
                {
                    var sql = $"update {nameof(Navigation)} set {nameof(Navigation.IsDelete)}=1 where {nameof(Navigation.NavigationId)}={id}";

                    if (_navigationService.ExecuteSql(sql) > 0)
                    {
                        result.Code = (int)ResultCode.Success;
                        result.Msg = "删除成功";
                    }
                }
            }

            return Json(result);
        }
    }
}