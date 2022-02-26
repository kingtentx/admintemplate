using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace King.Api.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ArticleDto
    {
        /// <summary>
        /// 
        /// </summary>
        public int ArticleID { get; set; }
        /// <summary>
        /// 分类ID
        /// </summary>
        public int CategoryID { get; set; }
        /// <summary>
        /// 标题
        /// </summary>      
        public string Title { get; set; }
        /// <summary>
        /// 关键字
        /// </summary>        
        public string Keyword { get; set; }

        /// <summary>
        /// 详情
        /// </summary>      
        public string Detail { get; set; }
        /// <summary>
        /// 作者
        /// </summary>       
        public string Author { get; set; }

        /// <summary>
        /// 链接
        /// </summary>

        public string LinkUrl { get; set; }
        /// <summary>
        /// 图片
        /// </summary>

        public string ImageUrl { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; } = 0;
        /// <summary>
        /// 浏览量
        /// </summary>
        public int ViewCount { get; set; } = 0;

        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>       
        public string UpdateBy { get; set; }
    }
}
