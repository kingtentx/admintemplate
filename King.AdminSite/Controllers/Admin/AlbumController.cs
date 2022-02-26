using AutoMapper;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using King.AdminSite.Models;
using King.Data;
using King.Helper;
using King.Interface;
using King.Utils;
using King.Utils.Enums;

namespace King.AdminSite.Controllers
{
    [Authorize]
    public class AlbumController : AdminBaseController
    {       
        private IBllService<Album> _albumService;
        private IBllService<Tags> _tagsService;
        private IPermission _permission;
        private IMapper _mapper;
        private IWebHostEnvironment _hostingEnv;
        private readonly IConfiguration _config;

        public AlbumController(IWebHostEnvironment hostingEnv, IConfiguration configuration,
            IBllService<Album> albumService, IBllService<Tags> tagsService, IPermission permission, IMapper mapper)
        {
            _albumService = albumService;
            _permission = permission;
            _mapper = mapper;
            _tagsService = tagsService;
            _hostingEnv = hostingEnv;
            _config = configuration;
        }
        public IActionResult Index()
        {
            //if (!_permission.CheckPermission(LoginUser, MenuCode.Management_Album, PermissionType.View))
            //{
            //    return Content("无访问权限");
            //}          

            //ViewData[PageCode.PAGE_Button_Add] = _permission.CheckPermission(LoginUser, MenuCode.Management_Album, PermissionType.Add);
            //ViewData[PageCode.PAGE_Button_Edit] = _permission.CheckPermission(LoginUser, MenuCode.Management_Album, PermissionType.Edit);
            //ViewData[PageCode.PAGE_Button_Delete] = _permission.CheckPermission(LoginUser, MenuCode.Management_Album, PermissionType.Delete);         

            return View();

        }

        [HttpGet]
        public async Task<JsonResult> GetList(int pageIndex = 1, int pageSize = 10)
        {

            AjaxResultList result = new AjaxResultList();

            //if (!_permission.CheckPermission(LoginUser, MenuCode.Management_Album, PermissionType.View))
            //{
            //    result.Code = (int)ResultCode.Nopermit;
            //    result.Msg = "无操作权限";

            //    return Json(result);
            //}

            var title = HttpContext.Request.Query["txtTitle"].ToString().Trim() ?? "";

            var where = LambdaHelper.True<Album>().And(p => p.IsDelete == false);

            if (!string.IsNullOrWhiteSpace(title))
                where = where.And(p => p.Title.Contains(title));

            //var query = await _albumService.GetListAsync(where, p => p.AlbumId, pageIndex, pageSize);
            //var data = _mapper.Map(query.List, new List<AlbumModel>());

            var tagslist = await _tagsService.GetListAsync(p => p.TagsType == (int)TagsType.ImageList && p.IsDelete == false);
            var query = _albumService.GetModel(where, p => p.AlbumId, pageIndex, pageSize);

            var data = (from q in query.Queryable
                        select new AlbumModel()
                        {
                            AlbumId = q.AlbumId,
                            ImageUrl = q.ImageUrl,
                            Title = q.Title,
                            Description = q.Description,
                            TagsId = q.TagsId,
                            IsActive = q.IsActive,
                            CreateTime = q.CreateTime,
                            TagsList = _mapper.Map<List<TagsModel>>(tagslist)
                        }).ToList();

            result.Code = (int)ResultCode.Success;
            result.Msg = "成攻";
            result.Count = query.Count;
            result.Data = data;

            return Json(result);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            //if (!_permission.CheckPermission(LoginUser, MenuCode.Management_Album, PermissionType.View))
            //{
            //    return Content("无访问权限");
            //}        

            var tagslist = await _tagsService.GetListAsync(p => p.TagsType == (int)TagsType.ImageList && p.IsDelete == false);

            AlbumModel model = new AlbumModel();

            if (id > 0)
            {
                var query = await _albumService.GetOneAsync(id);
                model = _mapper.Map<AlbumModel>(query);
                model.TagsList = _mapper.Map<List<TagsModel>>(tagslist);

                return View(model);
            }
            else
            {
                model.TagsList = _mapper.Map<List<TagsModel>>(tagslist);
            }

            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        public async Task<ActionResult> EditPost(int id, [FromForm] AlbumModel input)
        {
            AjaxResult result = new AjaxResult() { Code = (int)ResultCode.ParmsError, Msg = "失败" };

            if (id > 0)
            {
                //if (!_permission.CheckPermission(LoginUser, MenuCode.Management_Album, PermissionType.Edit))
                //{
                //    result.Code = (int)ResultCode.Nopermit;
                //    result.Msg = "无操作权限";

                //    return Json(result);
                //}

                var editmodel = await _albumService.GetOneAsync(id);
                editmodel.TagsId = input.TagsId;
                editmodel.Title = input.Title;
                editmodel.Author = input.Author;
                editmodel.Description = input.Description;
                editmodel.ImageUrl = input.ImageUrl;
                editmodel.IsActive = input.IsActive;
                editmodel.UpdateBy = LoginUser.UserName;
                editmodel.UpdateTime = DateTime.Now;

                if (await _albumService.UpdateAsync(editmodel))
                {
                    result.Code = (int)ResultCode.Success;
                    result.Msg = "修改成功";
                }
            }
            else
            {
                //if (!_permission.CheckPermission(LoginUser, MenuCode.Management_Album, PermissionType.Add))
                //{
                //    result.Code = (int)ResultCode.Nopermit;
                //    result.Msg = "无操作权限";

                //    return Json(result);
                //}

                var model = _mapper.Map<Album>(input);
                model.TagsType = (int)TagsType.ImageList;
                model.CreateBy = LoginUser.UserName;
                model.CreateTime = DateTime.Now;

                if ((await _albumService.AddAsync(model)).AlbumId > 0)
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

            //if (!_permission.CheckPermission(LoginUser, MenuCode.Management_Album, PermissionType.Delete))
            //{
            //    result.Code = (int)ResultCode.Nopermit;
            //    result.Msg = "无操作权限";

            //    return Json(result);
            //}

            if (isAll == 0)
            {
                if (id > 0)
                {
                    var sql = $"update {nameof(Album)}  set {nameof(Album.IsDelete)}=1 where {nameof(Album.AlbumId)} = ({id})";

                    if (await _albumService.ExecuteSqlAsync(sql) > 0)
                    {
                        log.Info($"删除album:id={id}");
                        result.Code = (int)ResultCode.Success;
                        result.Msg = "删除成功";
                    }
                }
            }
            else
            {
                var ids = Request.Form["ids[]"].ToArray();
                var strNums = string.Join(",", Array.ConvertAll(ids, int.Parse));
                var sql = $"update {nameof(Album)} set {nameof(Album.IsDelete)}=1 where {nameof(Album.AlbumId)} in ({strNums})";

                if (await _albumService.ExecuteSqlAsync(sql) > 0)
                {
                    log.Info($"批量删除album:id={strNums}");
                    result.Code = ResultCode.Success.GetHashCode();
                    result.Msg = "删除成功";
                }
            }

            return Json(result);
        }

        [HttpGet]
        public async Task<ActionResult> BatchUpload()
        {
            //if (!_permission.CheckPermission(LoginUser, MenuCode.Management_Album, PermissionType.View))
            //{
            //    return Content("无访问权限");
            //}        

            var tagslist = await _tagsService.GetListAsync(p => p.TagsType == (int)TagsType.ImageList && p.IsDelete == false);
            AlbumModel model = new AlbumModel()
            {
                TagsList = _mapper.Map<List<TagsModel>>(tagslist)
            };
            return View(model);
        }

        [HttpPost, ActionName("BatchUpload")]
        public async Task<ActionResult> BatchUploadPost([FromForm] AlbumModel input)
        {
            AjaxResult result = new AjaxResult() { Code = (int)ResultCode.ParmsError, Msg = "失败" };

            //if (!_permission.CheckPermission(LoginUser, MenuCode.Management_Album, PermissionType.Add))
            //{
            //    result.Code = (int)ResultCode.Nopermit;
            //    result.Msg = "无操作权限";

            //    return Json(result);
            //}

            if (input.ImageList.Count == 0)
            {
                result.Code = (int)ResultCode.Fail;
                result.Msg = "请上传图片";
                return Json(result);
            }

            List<Album> list = new List<Album>();

            foreach (var img in input.ImageList)
            {
                var model = _mapper.Map<Album>(input);
                model.ImageUrl = img;
                model.TagsType = (int)TagsType.ImageList;
                model.CreateBy = LoginUser.UserName;
                model.CreateTime = DateTime.Now;

                list.Add(model);
            }

            if ((await _albumService.AddAsync(list)))
            {
                result.Code = (int)ResultCode.Success;
                result.Msg = "添加成功";
            }

            return Json(result);
        }

    }

}