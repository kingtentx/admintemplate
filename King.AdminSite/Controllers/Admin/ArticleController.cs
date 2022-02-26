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
    public class ArticleController : AdminBaseController
    {     
        private IBllService<Article> _articleService;
        private IBllService<Data.Tags> _tagsService;
        private IPermission _permission;
        private IMapper _mapper;
        private IWebHostEnvironment _hostingEnv;
        private readonly IConfiguration _config;

        public ArticleController(IWebHostEnvironment hostingEnv, IConfiguration configuration, IBllService<Article> articleService, IBllService<Data.Tags> tagsService, IPermission permission, IMapper mapper)
        {
            _articleService = articleService;
            _permission = permission;
            _mapper = mapper;
            _tagsService = tagsService;
            _hostingEnv = hostingEnv;
            _config = configuration;
        }
        public IActionResult Index()
        {
            if (!_permission.CheckPermission(LoginUser, MenuCode.Content_Article, PermissionType.View))
            {
                return Content("无访问权限");
            }
           

            ViewData[PageCode.PAGE_Button_Add] = _permission.CheckPermission(LoginUser, MenuCode.Content_Article, PermissionType.Add);
            ViewData[PageCode.PAGE_Button_Edit] = _permission.CheckPermission(LoginUser, MenuCode.Content_Article, PermissionType.Edit);
            ViewData[PageCode.PAGE_Button_Delete] = _permission.CheckPermission(LoginUser, MenuCode.Content_Article, PermissionType.Delete);
            ViewData[PageCode.PAGE_Button_Import] = _permission.CheckPermission(LoginUser, MenuCode.Content_Article, PermissionType.Import);
            ViewData[PageCode.PAGE_Button_Export] = _permission.CheckPermission(LoginUser, MenuCode.Content_Article, PermissionType.Export);

            return View();

        }

        [HttpGet]
        public async Task<JsonResult> GetList(int pageIndex = 1, int pageSize = 10)
        {

            AjaxResultList result = new AjaxResultList();

            if (!_permission.CheckPermission(LoginUser, MenuCode.Content_Article, PermissionType.View))
            {
                result.Code = (int)ResultCode.Nopermit;
                result.Msg = "无操作权限";

                return Json(result);
            }

            var title = HttpContext.Request.Query["txtTitle"].ToString().Trim() ?? "";

            var where = LambdaHelper.True<Article>().And(p => p.IsDelete == false);

            if (!string.IsNullOrWhiteSpace(title))
                where = where.And(p => p.Title.Contains(title));

            //var query = await _articleService.GetListAsync(where, p => p.ArticleId, pageIndex, pageSize);
            //var data = _mapper.Map(query.List, new List<ArticleModel>());

            var tagslist = await _tagsService.GetListAsync(p => p.TagsType == (int)TagsType.ImageTextList && p.IsDelete == false);

            //查询数据不执行SQL
            var query = _articleService.GetModel(where, p => p.ArticleId, pageIndex, pageSize);
            //执行SQL不读取文章详情等数据
            var data = (from q in query.Queryable
                        select new ArticleModel()
                        {
                            ArticleId = q.ArticleId,
                            Title = q.Title,
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
            if (!_permission.CheckPermission(LoginUser, MenuCode.Content_Article, PermissionType.View))
            {
                return Content("无访问权限");
            }

            var tagslist = await _tagsService.GetListAsync(p => p.TagsType == (int)TagsType.ImageTextList && p.IsDelete == false);

            ArticleModel model = new ArticleModel();

            if (id > 0)
            {
                var query = await _articleService.GetOneAsync(id);
                model = _mapper.Map<ArticleModel>(query);
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
        public async Task<ActionResult> EditPost(int id, [FromForm] ArticleModel input)
        {
            AjaxResult result = new AjaxResult() { Code = (int)ResultCode.ParmsError, Msg = "失败" };

            if (id > 0)
            {
                if (!_permission.CheckPermission(LoginUser, MenuCode.Content_Article, PermissionType.Edit))
                {
                    result.Code = (int)ResultCode.Nopermit;
                    result.Msg = "无操作权限";

                    return Json(result);
                }

                var editmodel = await _articleService.GetOneAsync(id);
                editmodel.TagsId = input.TagsId;
                editmodel.Title = input.Title;
                editmodel.Author = input.Author;
                editmodel.Detail = input.Detail;
                editmodel.ImageUrl = input.ImageUrl;
                editmodel.IsActive = input.IsActive;
                editmodel.UpdateBy = LoginUser.UserName;
                editmodel.UpdateTime = DateTime.Now;

                if (await _articleService.UpdateAsync(editmodel))
                {
                    result.Code = (int)ResultCode.Success;
                    result.Msg = "修改成功";
                }
            }
            else
            {
                if (!_permission.CheckPermission(LoginUser, MenuCode.Content_Article, PermissionType.Add))
                {
                    result.Code = (int)ResultCode.Nopermit;
                    result.Msg = "无操作权限";

                    return Json(result);
                }

                var model = _mapper.Map<Article>(input);
                model.TagsType = (int)TagsType.ImageTextList;
                model.CreateBy = LoginUser.UserName;
                model.CreateTime = DateTime.Now;

                if ((await _articleService.AddAsync(model)).ArticleId > 0)
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

            if (!_permission.CheckPermission(LoginUser, MenuCode.Content_Article, PermissionType.Delete))
            {
                result.Code = (int)ResultCode.Nopermit;
                result.Msg = "无操作权限";

                return Json(result);
            }

            if (isAll == 0)
            {
                if (id > 0)
                {
                    //MySqlParameter[] parameters = new[]{
                    //    new MySqlParameter("Id", id)
                    //};
                    //SqlParameter[] parameters = new[]{
                    //    new SqlParameter("Id", id)
                    //};

                    var sql = $"update {nameof(Article)}  set {nameof(Article.IsDelete)}=1 where {nameof(Article.ArticleId)} = ({id})";

                    if (await _articleService.ExecuteSqlAsync(sql) > 0)
                    {
                        log.Info($"删除article：id={id}，操作人：{LoginUser.UserName}");
                        result.Code = (int)ResultCode.Success;
                        result.Msg = "删除成功";
                    }

                }
            }
            else
            {
                var ids = Request.Form["ids[]"].ToString();
                var sql = $"update {nameof(Article)} set {nameof(Article.IsDelete)}=1 where {nameof(Article.ArticleId)} in ({ids})";

                if (await _articleService.ExecuteSqlAsync(sql) > 0)
                {
                    log.Info($"批量删除Article:id={ids}，操作人：{LoginUser.UserName}");
                    result.Code = ResultCode.Success.GetHashCode();
                    result.Msg = "删除成功";
                }
            }

            return Json(result);
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult Export()
        {

            AjaxResult result = new AjaxResult() { Code = (int)ResultCode.ParmsError, Msg = "参数不全" };

            if (!_permission.CheckPermission(LoginUser, MenuCode.Content_Article, PermissionType.Export))
            {
                result.Code = (int)ResultCode.Nopermit;
                result.Msg = "无操作权限";

                return Json(result);
            }

            var rootPath = _hostingEnv.WebRootPath;
            var localPath = _config["ExcelConfig:DownloadPath"] + "/";
            string sFileName = $"article_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
            var downloadPath = Path.Combine(rootPath + localPath, sFileName);

            var title = HttpContext.Request.Query["txtTitle"];

            var where = LambdaHelper.True<Article>();
            where = where.And(p => p.IsDelete == false);

            if (!string.IsNullOrWhiteSpace(title))
                where = where.And(p => p.Title.Contains(title));

            var list = _articleService.GetModel(where, p => p.ArticleId);


            Dictionary<string, string> cellheader = new Dictionary<string, string> {
                            { nameof(Article.ArticleId), "ID" },
                            { nameof(Article.Title), "标题" },
                            { nameof(Article.CreateBy), "创建人" },
                            { nameof(Article.CreateTime) , "创建时间" }
                      };


            var b = ExcelHelper.ListToExcel(cellheader, list.ToList(), downloadPath);
            if (b.Success)
            {
                result.Code = ResultCode.Success.GetHashCode();
                result.Msg = localPath + sFileName;
            }
            else
            {
                result.Code = ResultCode.ServerError.GetHashCode();
                result.Msg = b.Message;
            }

            return Json(result);
        }

        [HttpGet]
        public ActionResult Import()
        {
            return View();
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Import(IFormFile file)
        {
            AjaxResult result = new AjaxResult() { Code = (int)ResultCode.Fail, Msg = "失败" };

            if (!_permission.CheckPermission(LoginUser, MenuCode.Content_Article, PermissionType.Import))
            {
                result.Code = (int)ResultCode.Nopermit;
                result.Msg = "无操作权限";

                return Json(result);
            }

            var uploadPath = _hostingEnv.WebRootPath + _config["ExcelConfig:UploadPath"] + "/";
            var saveName = "article" + DateTime.Now.Ticks.ToString() + Path.GetExtension(file.FileName).ToLower();

            var s = Common.Save(file.OpenReadStream(), uploadPath, saveName);

            Dictionary<string, string> cellheader = new Dictionary<string, string> {
                            { nameof(Article.Title), "标题" },
                            { nameof(Article.Author), "作者" },
                            { nameof(Article.Detail), "内容" },
                            { nameof(Article.CreateTime) , "时间" }
                      };

            var item = ExcelHelper.ExcelToList<Article>(cellheader, uploadPath + saveName);

            if (item.List != null)
            {
                var b = _articleService.Add(item.List);
                if (b)
                {
                    result.Code = ResultCode.Success.GetHashCode();
                    result.Msg = "成功";
                }
                else
                {
                    result.Msg = item.Message;
                }
            }
            return Json(result);
        }
    }

}