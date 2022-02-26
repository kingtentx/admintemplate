using AutoMapper;
using King.Api.Models;
using King.Data;
using King.Helper;
using King.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace King.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private IMapper _mapper;
        private IBllService<Tags> _tagsService;
        private IBllService<Article> _articleService;

        public NewsController(IMapper mapper, IBllService<Tags> tagService, IBllService<Article> articleService)
        {
            _mapper = mapper;
            _tagsService = tagService;
            _articleService = articleService;
        }

        /// <summary>
        /// Tags标签
        /// </summary>
        /// <returns></returns>
        [HttpGet]

        public async Task<ActionResult> GetTags()
        {
            var data = await _tagsService.GetListAsync(p => p.IsActive == true, p => p.Sort, true);
            //var data = _mapper.Map(query, new List<CategoryDto>());
            //TreeModel tree = new TreeModel();
            //var data = Utils.CreateChildTree(query.ToList(), tree);

            return Ok(data);
        }

        /// <summary>
        /// 新闻列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetNewsList(int tagid = 0, int pageIndex = 1, int pageSize = 10)
        {
            var where = LambdaHelper.True<Article>().And(p => p.IsDelete == false && p.IsActive == true);
            if (tagid > 0)
            {
                where = where.And(p => p.TagsId == tagid);
            }

            var query = await _articleService.GetListAsync(where, p => p.Sort, pageIndex, pageSize);

            var data = _mapper.Map(query.List, new List<ArticleDto>());

            AjaxResultList result = new AjaxResultList();
            result.Code = (int)ResultCode.Success;
            result.Count = query.Count;
            result.Data = data;

            return Ok(result);
        }
    }
}
