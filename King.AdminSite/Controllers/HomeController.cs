using log4net;
using AutoMapper;
using King.AdminSite.Models;
using King.Data;
using King.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using King.Utils.Enums;
using King.Helper;

namespace King.AdminSite.Controllers
{
    public class HomeController : Controller
    {
        private ILog log = LogManager.GetLogger(Startup.logRepository.Name, typeof(HomeController));
        private ICacheService _cache;
        private IMapper _mapper;
        private IBllService<SiteConfig> _siteService;
        private IBllService<Navigation> _navigationService;
        private IBllService<Tags> _tagsService;
        private IBllService<Article> _articleService;
        private IBllService<Album> _albumService;

        public HomeController(ICacheService cache, IMapper mapper,
              IBllService<SiteConfig> siteService,
             IBllService<Navigation> navigationService,
             IBllService<Tags> tagsService,
             IBllService<Article> articleService,
             IBllService<Album> albumService
           )
        {
            _cache = cache;
            _mapper = mapper;
            _siteService = siteService;
            _navigationService = navigationService;
            _tagsService = tagsService;
            _articleService = articleService;
            _albumService = albumService;
        }

        /// <summary>
        /// 导航缓存键
        /// </summary>
        public const string CACHE_NVATGATION = "CACHE_NVATGATION";
        /// <summary>
        /// 网站信息
        /// </summary>
        public const string CACHE_SITEINFO = "CACHE_SITEINFO";


        public async Task<IActionResult> Index(string path, int id = 0)
        {
            // 网站配置信息
            ViewBag.SiteInfo = await GetSiteInfo();          

            return View();
        }

        /// <summary>
        /// 获取文章列表
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public JsonResult GetArticles(int tid, int pageIndex = 1, int pageSize = 6)
        {
            AjaxResultList result = new AjaxResultList();

            var where = LambdaHelper.True<Article>().And(p => p.IsActive == true && p.IsDelete == false);

            if (tid > 0)
                where = where.And(p => p.TagsId == tid);

            var query = _articleService.GetModel(where, p => p.ArticleId, pageIndex, pageSize);

            var data = (from q in query.Queryable
                        select new ArticleOutput
                        {
                            ArticleId = q.ArticleId,
                            Title = q.Title,
                            ImageUrl = q.ImageUrl,
                            CreateTime = q.CreateTime.Value.ToString("yyyy年MM月dd日"),
                            Description = StringHelper.RemoveHtml(q.Detail).CutString(150)
                        }).ToList();

            result.Code = (int)ResultCode.Success;
            result.Msg = "成攻";
            result.Count = query.Count;
            result.Data = data;

            return Json(result);
        }


        /// <summary>
        /// 获取文章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Article(int id = 0)
        {
            // 网站配置信息
            ViewBag.SiteInfo = await GetSiteInfo();

            #region 导航数据
            var navlist = this.GetNavigationList();

            //显示导航
            ViewData["NavList"] = navlist.Where(p => p.IsShow == true && p.IsActive == true).OrderBy(p => p.Sort).ToList();

            #endregion

            var query = await _articleService.GetOneAsync(id);

            if (query == null)
            {
                return Redirect("/home/404");
            }

            var model = _mapper.Map<ArticleModel>(query);
            ViewData["Title"] = (await _tagsService.GetOneAsync(model.TagsId)).TagsName;
            return View(model);
        }

        /// <summary>
        /// 获取相册列表
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public JsonResult GetAlbums(int tid, int pageIndex = 1, int pageSize = 6)
        {
            AjaxResultList result = new AjaxResultList();

            var where = LambdaHelper.True<Album>().And(p => p.IsActive == true && p.IsDelete == false);

            if (tid > 0)
                where = where.And(p => p.TagsId == tid);

            var query = _albumService.GetModel(where, p => p.AlbumId, pageIndex, pageSize);

            var data = (from q in query.Queryable
                        select new AlbumOutput
                        {
                            AlbumId = q.AlbumId,
                            Title = q.Title,
                            ImageUrl = q.ImageUrl,
                            CreateTime = q.CreateTime.Value.ToString("yyyy年MM月dd日")
                            //Description = StringHelper.RemoveHtml(q.Detail).CutString(150)
                        }).ToList();


            result.Code = (int)ResultCode.Success;
            result.Msg = "成攻";
            result.Count = query.Count;
            result.Data = data;

            return Json(result);
        }

       

        /// <summary>
        /// 导航列表
        /// </summary>
        /// <returns></returns>
        private List<NavigationModel> GetNavigationList()
        {
            List<NavigationModel> navlist = new List<NavigationModel>();

            if (_cache.Exists(CACHE_NVATGATION))
            {
                navlist = _cache.Get<List<NavigationModel>>(CACHE_NVATGATION);
            }
            else
            {
                var navigationList = _navigationService.GetModel(p => p.IsActive == true, p => p.Sort).ToList();
                navlist = _mapper.Map<List<NavigationModel>>(navigationList);
                _cache.Add(CACHE_NVATGATION, navlist);
            }

            return navlist;
        }

        /// <summary>
        /// 网站信息
        /// </summary>
        /// <returns></returns>
        private async Task<SiteConfigModel> GetSiteInfo()
        {
            SiteConfigModel siteinfo = new SiteConfigModel();

            if (_cache.Exists(CACHE_SITEINFO))
            {
                siteinfo = _cache.Get<SiteConfigModel>(CACHE_SITEINFO);
            }
            else
            {
                var query = await _siteService.GetOneAsync(p => p.IsActive == true);
                if (query != null)
                {
                    siteinfo = _mapper.Map<SiteConfigModel>(query);
                    _cache.Add(CACHE_SITEINFO, siteinfo);
                }
            }

            return siteinfo;
        }

        [ActionName("404")]
        public IActionResult Error()
        {
            return View();
        }

    }
}
