using AutoMapper;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using King.AdminSite.Models;
using King.Data;
using King.Helper;
using King.Interface;
using King.Utils;
using King.Utils.Enums;
using King.Wecat;
using King.Wecat.Models;
using Newtonsoft.Json.Linq;

namespace King.AdminSite.Controllers
{
    [Authorize]
    public class WecatController : AdminBaseController
    {
        private IWebHostEnvironment _hostingEnv;
        private readonly IConfiguration _config;
        private IMapper _mapper;
        private IPermission _permission;
        private WeixinUtils _weixin;
        private string serverHost;
        private string imgUploadPath;

        private IBllService<Wx_Media> _wxmediaService;
        private IBllService<Wx_KeyWordsReply> _wxkeywordsreplyService;
        private IBllService<Wx_Keywords> _wxkeywordsService;
        private IBllService<Wx_Article> _wxarticleService;
        private IBllService<PictureGallery> _pictureService;
        private ICacheService _cache;

        public WecatController(IWebHostEnvironment hostingEnv, IConfiguration configuration, ICacheService cache, WeixinUtils weixin, IPermission permission, IMapper mapper,

            IBllService<Wx_Media> wxmediaService,
            IBllService<Wx_KeyWordsReply> wxkeywordsreplyService,
            IBllService<Wx_Keywords> wxkeywordsService,
            IBllService<Wx_Article> wxarticleService,
            IBllService<PictureGallery> pictureService

        )
        {
            _hostingEnv = hostingEnv;
            _config = configuration;
            serverHost = _config["App:ServerHost"];
            imgUploadPath = $"{_config["UploadConfig:Image:Path"]}/{DateTime.Now:yyyyMM}/{DateTime.Now:dd}/";
            _cache = cache;

            _mapper = mapper;
            _weixin = weixin;
            _permission = permission;
            _wxmediaService = wxmediaService;
            _wxkeywordsreplyService = wxkeywordsreplyService;
            _wxkeywordsService = wxkeywordsService;
            _wxarticleService = wxarticleService;
            _pictureService = pictureService;
        }


        #region 菜单 

        public IActionResult WxMenu()
        {
            if (!_permission.CheckPermission(LoginUser, MenuCode.Wecat_Menu, PermissionType.View))
            {
                return Content("无访问权限");
            }

            ViewData[PageCode.PAGE_Button_Edit] = _permission.CheckPermission(LoginUser, MenuCode.Wecat_Menu, PermissionType.Edit);

            return View();
        }

        [HttpGet]
        public JsonResult GetWxMenu()
        {
            AjaxResult result = new AjaxResult();
            List<WxMenuModel> list = new List<WxMenuModel>();
            if (_cache.Exists(CacheKey.WeiXin_Menu))
            {
                list = _cache.Get<List<WxMenuModel>>(CacheKey.WeiXin_Menu);
            }
            else
            {
                var url = WeixinApi.GetMenu(_weixin.GetToken());
                var data = Common.GetDownloadString(url);
                //log.Info("获取：" + data);
                var json = JsonConvert.DeserializeObject<WxJsonResult>(data);
                if (json.ErrorCodeValue > 0)
                {
                    log.Error("微信菜单获取错误:" + json.ToString());
                }
                else
                {
                    JsonTextReader reader = new JsonTextReader(new StringReader(data));
                    if (reader.Read())
                    {
                        JObject jsonObject = (JObject)JToken.ReadFrom(reader);
                        var buttons = jsonObject["menu"]["button"];
                        int id = 0;
                        int pid = 0;

                        foreach (var item in buttons)
                        {
                            id++;
                            var model = new WxMenuModel()
                            {
                                id = id,
                                pid = pid,
                                name = item["name"] != null ? item["name"].ToString() : "",
                                type = item["type"] != null ? item["type"].ToString() : "root",
                                value = GetValue(item),
                                sort = id,
                                level = 1
                            };

                            list.Add(model);

                            if (item["sub_button"] != null)
                            {
                                foreach (var sub_item in item["sub_button"])
                                {
                                    id++;
                                    var sub = new WxMenuModel()
                                    {
                                        id = id,
                                        pid = model.id,
                                        name = sub_item["name"] != null ? sub_item["name"].ToString() : "",
                                        type = sub_item["type"] != null ? sub_item["type"].ToString() : "",
                                        value = GetValue(sub_item),
                                        sort = id,
                                        level = 2
                                    };
                                    list.Add(sub);
                                }
                            }
                        }
                    }
                    _cache.Add(CacheKey.WeiXin_Menu, list);
                }
            }
            result.Code = (int)ResultCode.Success;
            result.Msg = "success";
            result.Data = list;
            return Json(result);

        }

        private string GetValue(JToken item)
        {
            var str = string.Empty;
            var key = item["type"] != null ? item["type"].ToString() : "";

            switch (key)
            {
                case "view":
                    str = item["url"].ToString();
                    break;
                case "click":
                    str = item["key"].ToString();
                    break;
                case "media_id":
                    str = item["media_id"].ToString();
                    break;
                default:
                    str = "";
                    break;
            }
            return str;
        }


        [HttpPost]
        public JsonResult SaveWxMenu(string menuList)
        {
            AjaxResult result = new AjaxResult() { Code = (int)ResultCode.ParmsError, Msg = "失败" };

            if (!_permission.CheckPermission(LoginUser, MenuCode.Wecat_Menu, PermissionType.Edit))
            {
                result.Code = (int)ResultCode.Nopermit;
                result.Msg = "无操作权限";

                return Json(result);
            }

            #region 处理数据
            var list = JsonConvert.DeserializeObject<List<WxMenuModel>>(menuList);
            var main_list = list.Where(p => p.pid == 0).OrderBy(p => p.sort).ToList();
            StringBuilder jsonStr = new StringBuilder();
            foreach (var main in main_list)
            {
                jsonStr.Append("{");
                jsonStr.AppendFormat("\"name\":\"{0}\",", main.name);
                var sub_list = list.Where(p => p.pid == main.id).OrderBy(p => p.sort).ToList();
                if (sub_list.Count > 0)
                {
                    jsonStr.Append("\"sub_button\": [");
                    foreach (var sub in sub_list)
                    {
                        jsonStr.Append("{");
                        jsonStr.AppendFormat("\"name\": \"{0}\",\"type\":\"{1}\",", sub.name, sub.type);
                        switch (sub.type)
                        {
                            case "view":
                                jsonStr.AppendFormat("\"url\": \"{0}\"", sub.value);
                                break;
                            case "click":
                                jsonStr.AppendFormat("\"key\": \"{0}\"", sub.value);
                                break;
                            case "media_id":
                                jsonStr.AppendFormat("\"media_id\": \"{0}\"", sub.value);
                                break;
                            default:
                                break;
                        }
                        jsonStr.Append("},");
                    }
                    jsonStr.Remove(jsonStr.Length - 1, 1);
                    jsonStr.Append("]");
                }
                else
                {
                    switch (main.type)
                    {
                        case "view":
                            jsonStr.AppendFormat("\"url\": \"{0}\"", main.value);
                            break;
                        case "click":
                            jsonStr.AppendFormat("\"key\": \"{0}\"", main.value);
                            break;
                        case "media_id":
                            jsonStr.AppendFormat("\"media_id\": \"{0}\"", main.value);
                            break;
                        default:
                            break;
                    }
                }
                jsonStr.Append("},");
            }
            jsonStr.Remove(jsonStr.Length - 1, 1);
            var jsonMenu = "{\"button\":[" + jsonStr.ToString() + "]}";

            #endregion


            var url = WeixinApi.CreateMenu(_weixin.GetToken());
            var data = Common.PostWeb(url, jsonMenu);
            var json = JsonConvert.DeserializeObject<WxJsonResult>(data);

            if (json.ErrorCodeValue > 0)
            {
                result.Code = ResultCode.Fail.GetHashCode();
                result.Msg = json.errmsg;
                log.Error("微信菜单保存错误===>" + json.errmsg);
            }
            else
            {
                result.Code = ResultCode.Success.GetHashCode();
                result.Msg = "保存成功";
                _cache.Remove(CacheKey.WeiXin_Menu);
                log.Info("微信菜单保存成功===>" + jsonMenu);
            }

            return Json(result);
        }

        #endregion

        #region  图文素材

        public IActionResult WxNewsList()
        {
            if (!_permission.CheckPermission(LoginUser, MenuCode.Wecat_Media, PermissionType.View))
            {
                return Content("无访问权限");
            }

            ViewData[PageCode.PAGE_Button_Add] = _permission.CheckPermission(LoginUser, MenuCode.Wecat_Media, PermissionType.Add);
            ViewData[PageCode.PAGE_Button_Edit] = _permission.CheckPermission(LoginUser, MenuCode.Wecat_Media, PermissionType.Edit);
            ViewData[PageCode.PAGE_Button_Delete] = _permission.CheckPermission(LoginUser, MenuCode.Wecat_Media, PermissionType.Delete);

            return View();
        }

        public IActionResult WxNewsInfo()
        {
            return View();
        }

        public async Task<IActionResult> NewsEdit(long? MeId)
        {
            if (!_permission.CheckPermission(LoginUser, MenuCode.Wecat_Media, PermissionType.View))
            {
                return Content("无访问权限");
            }

            if (MeId.HasValue == false) MeId = 0;

            var model = new WxNewsModel();
            var list = new List<WxArticleModel>();
            if (MeId > 0)
            {
                var news = await _wxmediaService.GetOneAsync(MeId.Value);
                if (news != null)
                {
                    model.MeId = news.MeId;

                    var query = await _wxarticleService.GetListAsync(p => p.MeId == news.MeId && p.IsDelete == false);
                    list = _mapper.Map<List<WxArticleModel>>(query);

                    model.Articles = list.OrderBy(p => p.Sort).ToList();
                }
            }
            else
            {
                var article = new WxArticleModel();
                list.Add(article);
                model.Articles = list;
            }
            return View(model);
        }

        [HttpPost, ActionName("NewsEdit")]
        public async Task<ActionResult> NewsEditPost([FromForm] WxArticleModel input)
        {
            AjaxResult result = new AjaxResult() { Code = (int)ResultCode.ParmsError, Msg = "失败" };

            if (input.Id > 0)
            {
                if (!_permission.CheckPermission(LoginUser, MenuCode.Wecat_Media, PermissionType.Edit))
                {
                    result.Code = (int)ResultCode.Nopermit;
                    result.Msg = "无操作权限";

                    return Json(result);
                }

                var editmodel = _wxarticleService.GetOne(input.Id);

                editmodel.Title = input.Title;
                editmodel.Author = input.Author;
                editmodel.Digest = input.Digest;
                editmodel.Content = input.Content;
                editmodel.ThumbMediaId = input.ThumbMediaId;
                editmodel.CoverUrl = input.CoverUrl;
                editmodel.ContentSourceUrl = input.ContentSourceUrl;

                editmodel.UpdateBy = LoginUser.UserName;
                editmodel.UpdateTime = DateTime.Now;

                //var news = new Wx_News()
                //{
                //    CoverUrl = input.CoverUrl,
                //    Description = input.Title,                  
                //};
                //await _wxnewsService.UpdateAsync(news);
                if (await _wxarticleService.UpdateAsync(editmodel))
                {
                    result.Code = (int)ResultCode.Success;
                    result.Msg = "修改成功";
                    result.Data = new { newId = editmodel.MeId, articleId = editmodel.Id };
                }
            }
            else
            {
                if (!_permission.CheckPermission(LoginUser, MenuCode.Wecat_Media, PermissionType.Add))
                {
                    result.Code = (int)ResultCode.Nopermit;
                    result.Msg = "无操作权限";
                    return Json(result);
                }

                var model = new Wx_Article();
                if (input.MeId > 0)
                {
                    model.MeId = input.MeId;

                    model = _mapper.Map(input, model);
                    model.CreateBy = LoginUser.UserName;
                    model.CreateTime = DateTime.Now;

                    if ((await _wxarticleService.AddAsync(model)).Id > 0)
                    {
                        result.Code = (int)ResultCode.Success;
                        result.Msg = "添加成功";
                        result.Data = new { newId = model.MeId, articleId = model.Id };
                    }
                }
                else
                {
                    model = _mapper.Map(input, model);
                    model.CreateBy = LoginUser.UserName;
                    model.CreateTime = DateTime.Now;

                    var list = new List<Wx_Article>();
                    list.Add(model);

                    var news = new Wx_Media()
                    {
                        MediaType = (int)MediaType.News,
                        CoverUrl = input.CoverUrl,
                        Introduction = input.Title,
                        Articles = list
                    };

                    var en = await _wxmediaService.AddAsync(news);
                    if (en.MeId > 0)
                    {
                        result.Code = (int)ResultCode.Success;
                        result.Msg = "添加成功";
                        result.Data = new { newId = model.MeId, articleId = en.Articles[0].Id };
                    }
                }
            }

            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> Uploadimg(IFormFile file)
        {
            if (!_permission.CheckPermission(LoginUser, MenuCode.Wecat_Media, PermissionType.Add))
            {
                return BadRequest("无操作权限");
            }

            if (file == null)
            {
                return BadRequest("请选择上传文件");
            }
            var oldFileName = file.FileName;//原文件名
            var extName = Path.GetExtension(file.FileName).ToLower(); //文件扩展名
            var size = file.Length;
            string[] imgExts = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
            if (!imgExts.Contains(extName))    //"图片格式";
            {
                return BadRequest("文件格式错误");
            }

            if (size > 1024 * 1024)    //"图片大小";
            {
                return BadRequest($"大小不能超过1M");
            }

            var saveName = DateTime.Now.Ticks.ToString() + extName;//保存的文件名         
            var serverPath = _hostingEnv.WebRootPath + imgUploadPath;

            using (var stream = file.OpenReadStream())
            {
                if (await Common.Save(stream, serverPath, saveName))
                {
                    //接口所上传的图片不占用公众号的素材库中图片数量的100000个的限制。图片仅支持jpg/png格式，大小必须在1MB以下
                    var url = WeixinApi.Uploadimg(_weixin.GetToken());

                    var data = await WeixinHelper.HttpUploadFile(url, serverPath + saveName, new MediaInput() { FileName = saveName });

                    if (data.Success)
                    {
                        System.Drawing.Image image = System.Drawing.Image.FromStream(stream);

                        var model = new PictureGalleryModel()
                        {
                            ImageName = oldFileName,
                            Url = serverHost + imgUploadPath + saveName,
                            ExtensionName = extName,
                            ThumbnailUrl = data.Media.Url,
                            Size = size,
                            Width = image.Width,
                            Height = image.Height,
                            CreateBy = LoginUser.UserName,
                            CreateTime = DateTime.Now
                        };

                        var dto = _mapper.Map<PictureGallery>(model);
                        await _pictureService.AddAsync(dto);

                        return Json(model);
                    }
                    else
                    {
                        return BadRequest("上传微信图片失败");
                    }
                }
                else
                {
                    return BadRequest("上传失败");
                }
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetWxNews(long id)
        {
            AjaxResult result = new AjaxResult();

            if (!_permission.CheckPermission(LoginUser, MenuCode.Wecat_Media, PermissionType.View))
            {
                result.Code = (int)ResultCode.Nopermit;
                result.Msg = "无操作权限";

                return Json(result);
            }

            var query = await _wxarticleService.GetOneAsync(id);

            result.Code = (int)ResultCode.Success;
            result.Msg = "成攻";
            result.Data = query;
            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> SortWxNews(string jsonData)
        {
            AjaxResult result = new AjaxResult();
            if (!_permission.CheckPermission(LoginUser, MenuCode.Wecat_Media, PermissionType.Edit))
            {
                result.Code = (int)ResultCode.Nopermit;
                result.Msg = "无操作权限";

                return Json(result);
            }

            if (!string.IsNullOrWhiteSpace(jsonData))
            {
                string sql = "";
                jsonData = jsonData.Trim();
                JsonTextReader reader = new JsonTextReader(new StringReader(jsonData));
                if (reader.Read())
                {
                    List<Dictionary<string, int>> dic = new List<Dictionary<string, int>>();
                    dic = JsonConvert.DeserializeObject<List<Dictionary<string, int>>>(jsonData);

                    foreach (var item in dic)
                    {
                        var strSql = $"update {nameof(Wx_Article).ToLower()} set {nameof(Wx_Article.Sort)}={item["sort"]} where {nameof(Wx_Article.Id)}={item["id"]};";
                        sql += strSql;
                    }
                    if (await _wxarticleService.ExecuteSqlAsync(sql) > 0)
                    {
                        result.Code = (int)ResultCode.Success;
                        result.Msg = "更新成功";
                    }
                }
            }

            return Json(result);
        }

        /// <summary>
        /// 修改素材的时候 ，每次只能修改某一素材的其中一条，如果要删除或者添加一条就没办法了，很蛋疼      
        /// 直接根据上传的获取的MEDIA_ID 进行删除，然后重新添加一次
        /// </summary>
        /// <param name="MeId"></param>
        /// <param name="articleId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> UploadWxNews(long? MeId, long? articleId)
        {
            AjaxResult result = new AjaxResult();
            if (!_permission.CheckPermission(LoginUser, MenuCode.Wecat_Media, PermissionType.Add))
            {
                result.Code = (int)ResultCode.Nopermit;
                result.Msg = "无操作权限";

                return Json(result);
            }

            if (MeId.HasValue == false) MeId = 0;
            if (articleId.HasValue == false) articleId = 0;

            if (MeId > 0)//上传新素材
            {
                var news = await _wxmediaService.GetOneAsync(MeId.Value);

                if (!string.IsNullOrWhiteSpace(news.MediaId))
                {
                    var del_url = WeixinApi.DeleteMaterial(_weixin.GetToken());
                    var strJson = await WeixinHelper.PostUrl(del_url, JsonConvert.SerializeObject(new { media_id = news.MediaId }));
                    if (JsonConvert.DeserializeObject<MediaResult>(strJson).Errcode == 0)
                    {
                        log.Info("删除图文media_id:" + news.MediaId);
                    }
                    else
                    {
                        result.Code = (int)ResultCode.Fail;
                        result.Msg = "图文素材上传失败，请重新上传";
                        return Json(result);
                    }
                }

                var query = await _wxarticleService.GetListAsync(p => p.MeId == news.MeId && p.IsDelete == false);
                var data = from a in query
                           orderby a.Sort
                           select new ArticleInput()
                           {
                               title = a.Title,
                               thumb_media_id = a.ThumbMediaId,
                               author = a.Author,
                               digest = a.Digest,
                               show_cover_pic = a.ShowCoverPic ? "1" : "0",
                               content = a.Content,
                               content_source_url = !string.IsNullOrWhiteSpace(a.ContentSourceUrl) ? a.ContentSourceUrl : serverHost,
                               need_open_comment = a.NeedOpenComment,
                               only_fans_can_comment = a.OnlyFansCanComment
                           };

                var body = JsonConvert.SerializeObject(new { articles = data });

                var url = WeixinApi.AddNews(_weixin.GetToken());

                var content = await WeixinHelper.PostUrl(url, body);
                log.Info(content);
                var res = JsonConvert.DeserializeObject<MediaResult>(content);
                if (res.Errcode == 0)
                {
                    var sql = $"update {nameof(Wx_Media).ToLower()} set {nameof(Wx_Media.MediaId).ToLower()}='{res.MediaId}' where {nameof(Wx_Media.MeId).ToLower()}={MeId}";
                    if (await _wxmediaService.ExecuteSqlAsync(sql) > 0)
                    {
                        result.Code = (int)ResultCode.Success;
                        result.Msg = "上传图文素材成功";
                        result.Data = res.MediaId;
                    }
                }
                else
                {
                    log.Error("上传图文素材失败:" + res.ToString());
                }

                #region 更新素材
                //只能更新其中一条
                //else
                //{
                //    var news = await _wxmediaService.GetOneAsync(MeId.Value);
                //    if (news != null)
                //    {
                //        //model.MeId = news.MeId;

                //        var a = await _wxarticleService.GetOneAsync(p => p.MeId == news.MeId && p.Id == articleId.Value);
                //        var data = new ArticleInput()
                //        {
                //            title = a.Title,
                //            thumb_media_id = a.ThumbMediaId,
                //            author = a.Author,
                //            digest = a.Digest,
                //            show_cover_pic = a.ShowCoverPic ? "1" : "0",
                //            content = a.Content,
                //            content_source_url = !string.IsNullOrWhiteSpace(a.ContentSourceUrl) ? a.ContentSourceUrl : serverHost,
                //            need_open_comment = a.NeedOpenComment,
                //            only_fans_can_comment = a.OnlyFansCanComment
                //        };

                //        var curr_article = new { media_id = news.MediaId, index = (a.Sort - 1).ToString(), articles = data };
                //        var body = JsonConvert.SerializeObject(curr_article);
                //        var url = WeixinApi.UpdateNews(_weixin.GetToken());
                //        var content = await WeixinHelper.PostUrl(url, body);
                //        log.Info(content);
                //        var res = JsonConvert.DeserializeObject<MediaResult>(content);
                //        if (res.Errcode == 0)
                //        {
                //            result.Code = (int)ResultCode.Success;
                //            result.Msg = "上传图文素材成功";
                //            result.Data = res.MediaId;
                //        }
                //        else
                //        {
                //            log.Error("更新图文素材失败:" + res.ToString());
                //        }
                //    }
                //}
                #endregion
            }
            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteWxNews(long MeId)
        {
            AjaxResult result = new AjaxResult();
            if (!_permission.CheckPermission(LoginUser, MenuCode.Wecat_Media, PermissionType.Delete))
            {
                result.Code = (int)ResultCode.Nopermit;
                result.Msg = "无操作权限";

                return Json(result);
            }
            var model = await _wxmediaService.GetOneAsync(MeId);
            if (!string.IsNullOrWhiteSpace(model.MediaId))
            {
                var url = WeixinApi.DeleteMaterial(_weixin.GetToken());
                var body = "{ \"media_id\":\"" + model.MediaId + "\"}";
                var content = Common.PostWeb(url, body);
                var res = JsonConvert.DeserializeObject<WxJsonResult>(content);
                if (res.errcode > 0)
                {
                    log.Error("删除图文素材失败:" + res.ToString());
                    result.Code = (int)ResultCode.Fail;
                    result.Msg = "图文素材删除失败，请重新操作";
                    return Json(result);
                }
            }
            var sql = $"update {nameof(Wx_Media).ToLower()} set {nameof(Wx_Media.IsDelete)}=1 where {nameof(Wx_Media.MeId)}={model.MeId}";
            if (await _wxmediaService.ExecuteSqlAsync(sql) > 0)
            {
                result.Code = (int)ResultCode.Success;
                result.Msg = "删除成功";
            }

            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteWxArticle(long MeId, long id)
        {
            AjaxResult result = new AjaxResult();
            if (!_permission.CheckPermission(LoginUser, MenuCode.Wecat_Media, PermissionType.Delete))
            {
                result.Code = (int)ResultCode.Nopermit;
                result.Msg = "无操作权限";

                return Json(result);
            }

            var query = await _wxarticleService.GetListAsync(p => p.MeId == MeId && p.IsDelete == false);
            if (query.Count == 1)
            {
                result.Code = (int)ResultCode.Fail;
                result.Msg = "至少保留一条图文消息";
            }
            else
            {
                var del_sql = $"update {nameof(Wx_Article)} set {nameof(Wx_Article.IsDelete)}= 1 where {nameof(Wx_Article.Id)}={id}";
                if (await _wxarticleService.ExecuteSqlAsync(del_sql) > 0)
                {
                    result.Code = (int)ResultCode.Success;
                    result.Msg = "删除成功";
                }
            }
            return Json(result);
        }
        #endregion

        #region  图片素材
        public IActionResult WxImageList()
        {
            if (!_permission.CheckPermission(LoginUser, MenuCode.Wecat_Media, PermissionType.View))
            {
                return Content("无访问权限");
            }

            ViewData[PageCode.PAGE_Button_Add] = _permission.CheckPermission(LoginUser, MenuCode.Wecat_Media, PermissionType.Add);
            ViewData[PageCode.PAGE_Button_Delete] = _permission.CheckPermission(LoginUser, MenuCode.Wecat_Media, PermissionType.Delete);

            return View();
        }

        public IActionResult WxImageInfo()
        {
            return View();
        }

        /// <summary>
        /// 上传微信图片素材
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> UploadWxImage(IFormFile file)
        {
            AjaxResult result = new AjaxResult();

            if (file == null)
            {
                result.Msg = "请选择上传文件";
                return Json(result);
            }
            var oldFileName = file.FileName;//原文件名
            var extName = Path.GetExtension(file.FileName).ToLower(); //文件扩展名
            var size = file.Length;

            string[] imgExts = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
            if (!imgExts.Contains(extName))    //"图片格式";
            {
                result.Msg = "文件格式错误";
                return Json(result);
            }

            if (size > 2 * 1024 * 1024)    //"图片大小限制2M";
            {
                result.Msg = $"大小不能超过{size}M";
                return Json(result);
            }

            var saveName = DateTime.Now.Ticks.ToString() + extName;//保存的文件名
            string uploadpath = "/upload/wxfile/image/";
            string serverPath = _hostingEnv.WebRootPath + uploadpath;

            using (var stream = file.OpenReadStream())
            {
                if (await Common.Save(stream, serverPath, saveName))
                {
                    var url = WeixinApi.AddMaterial(_weixin.GetToken(), MsgType.Image);
                    var input = new MediaInput() { FileName = oldFileName };
                    var res = await WeixinHelper.HttpUploadFile(url, serverPath + saveName, input);

                    if (res.Success)
                    {
                        System.Drawing.Image image = System.Drawing.Image.FromStream(stream);
                        var model = new Wx_Media()
                        {
                            MediaType = (int)MediaType.Image,
                            MediaId = res.Media.MediaId,
                            WxUrl = res.Media.Url,
                            FileName = oldFileName,
                            Url = serverHost + uploadpath + saveName,
                            ExtensionName = extName,
                            Size = size,
                            Width = image.Width,
                            Height = image.Height,
                            CreateBy = LoginUser.UserName,
                            CreateTime = DateTime.Now
                        };

                        await _wxmediaService.AddAsync(model);

                        result.Code = (int)ResultCode.Success;
                        result.Msg = res.Media.MediaId;
                    }
                    else
                    {
                        System.IO.File.Delete(serverPath + saveName);
                        result.Msg = "上传失败";
                    }
                }
            }

            return Json(result);
        }

        #endregion

        #region  视频、音频素材

        public IActionResult WxMediaInfo()
        {
            return View();
        }

        public IActionResult WxMediaList()
        {
            if (!_permission.CheckPermission(LoginUser, MenuCode.Wecat_Menu, PermissionType.View))
            {
                return Content("无访问权限");
            }

            ViewData[PageCode.PAGE_Button_Add] = _permission.CheckPermission(LoginUser, MenuCode.Wecat_Media, PermissionType.Add);
            ViewData[PageCode.PAGE_Button_Delete] = _permission.CheckPermission(LoginUser, MenuCode.Wecat_Media, PermissionType.Delete);


            return View();
        }

        public IActionResult MediaEdit(int? id, int? type)
        {
            if (!_permission.CheckPermission(LoginUser, MenuCode.Wecat_Media, PermissionType.View))
            {
                return Content("无访问权限");
            }

            if (id.HasValue == false) id = 0;

            WxMediaModel model = new WxMediaModel();
            model.MediaType = type.Value;

            if (id > 0)
            {
                var media = _wxmediaService.GetOne(id.Value);
                if (media == null)
                    return View(model);

                model = _mapper.Map<WxMediaModel>(media);
            }

            return View(model);
        }

        /// <summary>
        /// 图片（image）: 2M，支持bmp/png/jpeg/jpg/gif格式
        /// 语音（voice）：2M，播放长度不超过60s，mp3/wma/wav/amr格式
        /// 视频（video）：10MB，支持MP4格式
        /// 缩略图（thumb）：64KB，支持JPG格式
        /// </summary>
        /// <param name="type"></param>
        /// <param name="file"></param>
        /// <param name="fileName"></param>
        /// <param name="tempDirectory"></param>
        /// <param name="index"></param>
        /// <param name="total"></param>
        /// <param name="totalSize"></param>
        /// <param name="title"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> UploadWxMedia(int type, IFormFile file, string fileName, string tempDirectory, int index, int total, int totalSize, string title, string desc)
        {
            if (file == null)
            {
                return BadRequest("请选择上传文件");
            }

            var extName = Path.GetExtension(fileName).ToLower(); //文件扩展名        
            string mtype;

            if (type == (int)MediaType.Video)
            {
                string[] videoExts = { ".mp4" };
                if (!videoExts.Contains(extName))
                {
                    return BadRequest("文件格式错误");
                }
                if (totalSize > 10 * 1024 * 1024)    //"视频大小限制10M";
                {
                    return BadRequest("大小不能超过10M");
                }

                mtype = WeixinType.Video.ToString().ToLower();
            }
            else
            {
                string[] voiceExts = { ".mp3", ".wma", ".wav", ".amr" };
                if (!voiceExts.Contains(extName))
                {
                    return BadRequest("文件格式错误");
                }
                if (totalSize > 2 * 1024 * 1024)    //"音频大小限制2M";
                {
                    return BadRequest("大小不能超过2M");
                }

                mtype = WeixinType.Voice.ToString().ToLower();
            }
            string uploadpath = $"/upload/wxfile/{mtype}/";
            string tmpPath = tempDirectory + "/";
            string serverPath = _hostingEnv.WebRootPath + uploadpath;
            var saveName = DateTime.Now.Ticks.ToString() + extName;//保存的文件名

            try
            {
                using (var stream = file.OpenReadStream())
                {
                    if (await Common.Save(stream, serverPath + tmpPath, index.ToString()))
                    {
                        AjaxResult result = new AjaxResult();
                        bool mergeOk = false;

                        if (total == index)
                        {
                            mergeOk = await Common.FileMerge(tmpPath, serverPath, saveName);
                            if (mergeOk)
                            {
                                var url = WeixinApi.AddMaterial(_weixin.GetToken(), mtype);
                                var input = new MediaInput() { IsVideo = type == (int)MediaType.Video, FileName = saveName, Title = title, Introduction = desc };

                                var res = await WeixinHelper.HttpUploadFile(url, serverPath + saveName, input);

                                if (res.Success)
                                {
                                    var model = new Wx_Media()
                                    {
                                        MediaId = res.Media.MediaId,
                                        MediaType = type,
                                        Url = serverHost + uploadpath + saveName,
                                        FileName = title,
                                        Introduction = desc,
                                        ExtensionName = extName,
                                        Size = totalSize,
                                        CreateBy = LoginUser.UserName,
                                        CreateTime = DateTime.Now
                                    };

                                    var dto = await _wxmediaService.AddAsync(model);
                                }
                                else
                                {
                                    System.IO.File.Delete(serverPath + saveName);
                                    return BadRequest("上传素材服务器失败");
                                }

                                result.Code = (int)ResultCode.Success;
                                result.Msg = "保存成功";
                            }
                        }

                        result.Data = index;
                        return Json(result);
                    }
                    else
                    {
                        return BadRequest("上传失败");
                    }
                }
            }
            catch (Exception ex)
            {
                Directory.Delete(_hostingEnv.WebRootPath + uploadpath + tmpPath);//删除文件夹
                log.Error($"上传异常:{ex.Message}");
                return BadRequest("上传异常" + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="type">Type: news=1 image=2 video=3  voice=4</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> GetMediaList(int pageIndex = 1, int pageSize = 10, int type = 1)
        {
            AjaxResultList result = new AjaxResultList();

            if (!_permission.CheckPermission(LoginUser, MenuCode.Wecat_Media, PermissionType.View))
            {
                result.Code = (int)ResultCode.Nopermit;
                result.Msg = "无操作权限";

                return Json(result);
            }

            var where = LambdaHelper.True<Wx_Media>().And(p => p.MediaType == type && p.IsDelete == false);

            var query = await _wxmediaService.GetListAsync(where, p => p.MeId, pageIndex, pageSize);

            result.Code = (int)ResultCode.Success;
            result.Msg = "成攻";
            result.Count = query.Count;
            result.Data = query.List;
            return Json(result);
        }

        #endregion

        [HttpPost]
        public async Task<JsonResult> DeleteMedia(string mediaId, int type)
        {
            AjaxResult result = new AjaxResult();
            if (!_permission.CheckPermission(LoginUser, MenuCode.Wecat_Media, PermissionType.Delete))
            {
                result.Code = (int)ResultCode.Nopermit;
                result.Msg = "无操作权限";

                return Json(result);
            }

            var url = WeixinApi.DeleteMaterial(_weixin.GetToken());
            var body = "{ \"media_id\":\"" + mediaId + "\"}";
            var content = Common.PostWeb(url, body);
            var res = JsonConvert.DeserializeObject<WxJsonResult>(content);
            if (res.errcode == 0)
            {
                var mid = StringHelper.IsSafeSqlString(mediaId) ? mediaId : "0";

                var sql = $"update {nameof(Wx_Media).ToLower()} set {nameof(Wx_Media.IsDelete)}=1 where {nameof(Wx_Media.MediaId)}=\"{mid}\"";
                await _wxmediaService.ExecuteSqlAsync(sql);

                result.Code = (int)ResultCode.Success;
                result.Msg = "删除成功";
            }
            else
            {
                log.Error("删除素材失败:" + content);
            }
            return Json(result);
        }


        #region 关键词回复

        public async Task<IActionResult> AutoReply(int? id)
        {
            if (!_permission.CheckPermission(LoginUser, MenuCode.Wecat_AutoReply, PermissionType.View))
            {
                return Content("无访问权限");
            }
            var model = new WxKeyWordsReplyModel() { MsgType = MediaType.Text.GetHashCode().ToString() };

            if (id.HasValue == false) id = 0;
            if (id > 0)
            {
                var query = await _wxkeywordsreplyService.GetModel(p => p.IsSubscribe == false && p.KeyId == id, nameof(Wx_KeyWordsReply.WxKeyWordsList)).FirstOrDefaultAsync();
                if (query != null)
                {
                    model = _mapper.Map<WxKeyWordsReplyModel>(query);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> EditAutoReply(int? id, [FromForm] WxKeyWordsReplyModel model)
        {
            AjaxResult result = new AjaxResult();

            if (!_permission.CheckPermission(LoginUser, MenuCode.Wecat_AutoReply, PermissionType.Edit))
            {
                result.Code = (int)ResultCode.Nopermit;
                result.Msg = "无操作权限";

                return Json(result);
            }

            if (model.WxKeyWordsList.Count == 0)
            {
                result.Code = (int)ResultCode.Fail;
                result.Msg = "请填写关键词";
                return Json(result);
            }

            if (string.IsNullOrWhiteSpace(model.Content) && model.MeId == 0 && string.IsNullOrWhiteSpace(model.Image_MediaId) && string.IsNullOrWhiteSpace(model.Voice_MediaId) && string.IsNullOrWhiteSpace(model.Video_MediaId))
            {
                result.Code = (int)ResultCode.Fail;
                result.Msg = "请选择回复内容";
                return Json(result);
            }

            if (!string.IsNullOrWhiteSpace(model.Content))
            {
                model.MsgType = MediaType.Text.GetHashCode().ToString() + ",";
            }
            if (model.MeId > 0)
            {
                model.MsgType += MediaType.News.GetHashCode().ToString() + ",";
            }
            if (!string.IsNullOrWhiteSpace(model.Image_MediaId))
            {
                model.MsgType += MediaType.Image.GetHashCode().ToString() + ",";
            }
            if (!string.IsNullOrWhiteSpace(model.Voice_MediaId))
            {
                model.MsgType += MediaType.Voice.GetHashCode().ToString() + ",";
            }
            if (!string.IsNullOrWhiteSpace(model.Video_MediaId))
            {
                model.MsgType += MediaType.Video.GetHashCode().ToString() + ",";
            }

            if (id.HasValue == false) id = 0;

            var keywordsList = _mapper.Map<List<Wx_Keywords>>(model.WxKeyWordsList);

            var dto = new Wx_KeyWordsReply() { IsSubscribe = false, IsActive = model.IsActive, CreateTime = DateTime.Now };
            dto.KeyName = model.KeyName;
            dto.MsgType = model.MsgType;
            dto.Content = model.Content;
            dto.CoverUrl = model.CoverUrl;
            dto.Image_MediaId = model.Image_MediaId;
            dto.News_MediaId = model.News_MediaId;
            dto.Voice_MediaId = model.Voice_MediaId;
            dto.Video_MediaId = model.Video_MediaId;
            dto.ReplyType = model.ReplyType;
            dto.Remark = model.Remark;
            dto.WxKeyWordsList = keywordsList;

            if (id > 0) //update
            {
                dto.KeyId = id.Value;
                var oldArray = _wxkeywordsService.GetModel(p => p.KeyId == id.Value).AsNoTracking().Select(p => p.Id).ToArray();
                var newArray = keywordsList.Select(p => p.Id).ToArray();

                if (await _wxkeywordsreplyService.UpdateAsync(dto))
                {
                    var delArray = oldArray.Except(newArray).ToArray();//新菜单中没有的删除                                                                       
                    if (delArray.Length > 0)
                    {
                        _wxkeywordsService.Delete(p => delArray.Contains(p.Id));
                    }

                    result.Code = (int)ResultCode.Success;
                    result.Msg = "保存成功";
                }
            }
            else //add
            {
                if ((await _wxkeywordsreplyService.AddAsync(dto)).KeyId > 0)
                {
                    result.Code = (int)ResultCode.Success;
                    result.Msg = "保存成功";
                }
            }

            return Json(result);
        }

        //自动回复
        public IActionResult AutoReplyList()
        {
            if (!_permission.CheckPermission(LoginUser, MenuCode.Wecat_AutoReply, PermissionType.View))
            {
                return Content("无访问权限");
            }


            ViewData[PageCode.PAGE_Button_Add] = _permission.CheckPermission(LoginUser, MenuCode.Wecat_AutoReply, PermissionType.Add);
            ViewData[PageCode.PAGE_Button_Edit] = _permission.CheckPermission(LoginUser, MenuCode.Wecat_AutoReply, PermissionType.Edit);
            ViewData[PageCode.PAGE_Button_Delete] = _permission.CheckPermission(LoginUser, MenuCode.Wecat_AutoReply, PermissionType.Delete);

            return View();
        }

        //自动回复列表
        [HttpGet]
        public async Task<JsonResult> GetWxReplyList(int pageIndex = 1, int pageSize = 10)
        {
            AjaxResultList result = new AjaxResultList();

            if (!_permission.CheckPermission(LoginUser, MenuCode.Wecat_AutoReply, PermissionType.View))
            {
                result.Code = (int)ResultCode.Nopermit;
                result.Msg = "无操作权限";

                return Json(result);
            }
            var where = LambdaHelper.True<Wx_KeyWordsReply>().And(p => p.IsSubscribe == false);
            var query = await _wxkeywordsreplyService.GetListAsync(where, nameof(Wx_KeyWordsReply.WxKeyWordsList), p => p.KeyId, pageIndex, pageSize);

            result.Code = (int)ResultCode.Success;
            result.Msg = "成攻";
            result.Count = query.Count;
            result.Data = query.List;
            return Json(result);
        }

        //删除自动回复
        [HttpPost]
        public async Task<JsonResult> DeleteWxReply(int id)
        {
            AjaxResult result = new AjaxResult();
            if (!_permission.CheckPermission(LoginUser, MenuCode.Wecat_AutoReply, PermissionType.Delete))
            {
                result.Code = (int)ResultCode.Nopermit;
                result.Msg = "无操作权限";

                return Json(result);
            }

            if (await _wxkeywordsreplyService.DeleteAsync(id))
            {
                result.Code = (int)ResultCode.Success;
                result.Msg = "删除成功";
            }

            return Json(result);
        }
        #endregion

        #region 关注回复

        public IActionResult SubscribeReply()
        {
            if (!_permission.CheckPermission(LoginUser, MenuCode.Wecat_AutoReply, PermissionType.View))
            {
                return Content("无访问权限");
            }

            var model = new WxKeyWordsReplyModel() { MsgType = MediaType.Text.GetHashCode().ToString() };
            var data = _wxkeywordsreplyService.GetModel(p => p.IsSubscribe == true).FirstOrDefault();
            if (data != null)
            {
                model = _mapper.Map<WxKeyWordsReplyModel>(data);
            }

            return View(model);
        }


        [HttpPost]
        public async Task<JsonResult> EditSubscribeReply([FromForm] WxKeyWordsReplyModel model)
        {
            AjaxResult result = new AjaxResult();
            if (!_permission.CheckPermission(LoginUser, MenuCode.Wecat_AutoReply, PermissionType.Edit))
            {
                result.Code = (int)ResultCode.Nopermit;
                result.Msg = "无操作权限";

                return Json(result);
            }

            var dto = new Wx_KeyWordsReply() { IsSubscribe = true, IsActive = model.IsActive, CreateTime = DateTime.Now };

            switch (model.MsgType)
            {
                case MsgType.Text:
                    dto.MsgType = MediaType.Text.GetHashCode().ToString();
                    dto.Content = model.Content;
                    break;
                case MsgType.Image:
                    dto.MsgType = MediaType.Image.GetHashCode().ToString();
                    dto.CoverUrl = model.CoverUrl;
                    dto.Image_MediaId = model.Image_MediaId;
                    dto.Remark = model.Remark;
                    break;
                case MsgType.Voice:
                    dto.MsgType = MediaType.Voice.GetHashCode().ToString();
                    dto.Voice_MediaId = model.Voice_MediaId;
                    dto.Remark = model.Remark;
                    break;
                case MsgType.Video:
                    dto.MsgType = MediaType.Video.GetHashCode().ToString();
                    dto.Video_MediaId = model.Video_MediaId;
                    dto.Remark = model.Remark;
                    break;
            }

            var query = _wxkeywordsreplyService.GetModel(p => p.IsSubscribe == true).AsNoTracking().FirstOrDefault();
            if (query != null)
            {
                dto.KeyId = query.KeyId;

                if (await _wxkeywordsreplyService.UpdateAsync(dto))
                {
                    result.Code = (int)ResultCode.Success;
                    result.Msg = "保存成功";
                }
            }
            else
            {
                if ((await _wxkeywordsreplyService.AddAsync(dto)).KeyId > 0)
                {
                    result.Code = (int)ResultCode.Success;
                    result.Msg = "保存成功";
                }
            }

            return Json(result);
        }


        #endregion
    }
}