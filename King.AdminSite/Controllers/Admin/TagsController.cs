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
    public class TagsController : AdminBaseController
    {
        private IPermission _permission;
        private IMapper _mapper;
        private IBllService<Data.Tags> _tagsService;

        public TagsController(IMapper mapper, IPermission permission, IBllService<Tags> tagsService)
        {
            _mapper = mapper;
            _permission = permission;
            _tagsService = tagsService;
        }

        public IActionResult Index()
        {
            //if (!_permission.CheckPermission(LoginUser, MenuCode.Management_Tags, PermissionType.View))
            //{
            //    return Content("无访问权限");
            //}           

            //ViewData[PageCode.PAGE_Button_Add] = _permission.CheckPermission(LoginUser, MenuCode.Management_Tags, PermissionType.Add);
            //ViewData[PageCode.PAGE_Button_Edit] = _permission.CheckPermission(LoginUser, MenuCode.Management_Tags, PermissionType.Edit);
            //ViewData[PageCode.PAGE_Button_Delete] = _permission.CheckPermission(LoginUser, MenuCode.Management_Tags, PermissionType.Delete);

            return View();
        }

        public async Task<JsonResult> GetList()
        {

            AjaxResultList result = new AjaxResultList();

            //if (!_permission.CheckPermission(LoginUser, MenuCode.Management_Tags, PermissionType.View))
            //{
            //    result.Code = (int)ResultCode.Nopermit;
            //    result.Msg = "无操作权限";

            //    return Json(result);
            //}

            var query = await _tagsService.GetListAsync(p => p.IsDelete == false, p => p.TagsId);

            var data = _mapper.Map<List<TagsModel>>(query);

            result.Code = (int)ResultCode.Success;
            result.Msg = "成攻";
            result.Count = query.Count();
            result.Data = data;

            return Json(result);
        }

        public async Task<ActionResult> Edit(int id)
        {
            //if (!_permission.CheckPermission(LoginUser, MenuCode.Management_Tags, PermissionType.View))
            //{
            //    return Content("无访问权限");
            //}

            var model = new TagsModel();

            if (id > 0)
            {
                var query = await _tagsService.GetOneAsync(id);
                model = _mapper.Map<TagsModel>(query);

            }

            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        public async Task<ActionResult> EditPost(int id, TagsModel input)
        {
            AjaxResult result = new AjaxResult() { Code = (int)ResultCode.ParmsError, Msg = "失败" };

            if (id > 0)
            {
                //if (!_permission.CheckPermission(LoginUser, MenuCode.Management_Tags, PermissionType.Edit))
                //{
                //    result.Code = (int)ResultCode.Nopermit;
                //    result.Msg = "无操作权限";
                //    return Json(result);
                //}

                var editmodel = await _tagsService.GetOneAsync(id);

                editmodel.TagsName = input.TagsName;
                //editmodel.TagsType = input.TagsType; //编辑不可以修改标签类型
                editmodel.IsActive = input.IsActive;
                editmodel.Sort = input.Sort;
                editmodel.UpdateBy = LoginUser.UserName;
                editmodel.UpdateTime = DateTime.Now;

                if (await _tagsService.UpdateAsync(editmodel))
                {
                    result.Code = (int)ResultCode.Success;
                    result.Msg = "修改成功";
                }

            }
            else
            {
                //if (!_permission.CheckPermission(LoginUser, MenuCode.Management_Tags, PermissionType.Add))
                //{
                //    result.Code = (int)ResultCode.Nopermit;
                //    result.Msg = "无操作权限";
                //    return Json(result);
                //}

                var model = new Data.Tags();
                model.TagsName = input.TagsName;
                model.TagsType = input.TagsType;
                model.IsActive = input.IsActive;
                model.Sort = input.Sort;
                model.CreateBy = LoginUser.UserName;
                model.CreateTime = DateTime.Now;

                if ((await _tagsService.AddAsync(model)).TagsId > 0)
                {
                    result.Code = (int)ResultCode.Success;
                    result.Msg = "添加成功";
                }
            }

            return Json(result);
        }


        [HttpPost]
        public async Task<JsonResult> Delete(int isAll = 0, int id = 0)
        {
            AjaxResult result = new AjaxResult() { Code = (int)ResultCode.ParmsError, Msg = "参数不全" };

            //if (!_permission.CheckPermission(LoginUser, MenuCode.Management_Article, PermissionType.Delete))
            //{
            //    result.Code = (int)ResultCode.Nopermit;
            //    result.Msg = "无操作权限";

            //    return Json(result);
            //}

            if (isAll == 0)
            {
                if (id > 0)
                {
                    var sql = $"update {nameof(Tags)} set {nameof(Tags.IsDelete)}=1 where {nameof(Tags.TagsId)}={id}";

                    if (await _tagsService.ExecuteSqlAsync(sql) > 0)
                    {
                        log.Info($"删除Ct_Tags:id={id}");
                        result.Code = (int)ResultCode.Success;
                        result.Msg = "删除成功";
                    }

                }
            }
            else
            {
                var ids = Request.Form["ids[]"].ToArray();
                var strNums = string.Join(",", Array.ConvertAll(ids, int.Parse));
                var sql = $"update {nameof(Tags)} set {nameof(Tags.IsDelete)}=1 where {nameof(Tags.TagsId)} in ({strNums})";
                if (await _tagsService.ExecuteSqlAsync(sql) > 0)
                {
                    log.Info($"批量删除Ct_Tags:id={strNums}");
                    result.Code = ResultCode.Success.GetHashCode();
                    result.Msg = "删除成功";
                }
            }

            return Json(result);
        }
    }
}