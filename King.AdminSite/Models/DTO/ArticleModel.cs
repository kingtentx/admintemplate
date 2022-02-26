using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace King.AdminSite.Models
{
    public class ArticleModel
    {
        public int ArticleId { get; set; }
        /// <summary>
        /// 标题
        /// </summary>      
        public string Title { get; set; }
        /// <summary>
        /// 关键字
        /// </summary>        
        public string Keyword { get; set; }
        /// <summary>
        /// 描述
        /// </summary>       
        public string Description { get; set; }
        /// <summary>
        /// 详情
        /// </summary>      
        public string Detail { get; set; }
        /// <summary>
        /// 作者
        /// </summary>       
        public string Author { get; set; }
        /// <summary>
        /// 来源
        /// </summary>      
        public string Source { get; set; }


        public string SourceUrl { get; set; }
        /// <summary>
        /// 链接
        /// </summary>

        public string LinkUrl { get; set; }
        /// <summary>
        /// 图片
        /// </summary>

        public string ImageUrl { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public int TagsType { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public int TagsId { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; } = 0;
        /// <summary>
        /// 浏览量
        /// </summary>
        public int ViewCount { get; set; } = 0;
        /// <summary>
        /// 推荐位置
        /// </summary>       
        public string RecommendPosition { get; set; }
        /// <summary>
        /// 是否发表
        /// </summary>
        public bool IsActive { get; set; }

        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateBy { get; set; }
        /// <summary>
        /// 更新人
        /// </summary>       
        public string UpdateBy { get; set; }
        /// <summary>
        /// 标签名称
        /// </summary>
        public virtual string TagsName
        {
            get
            {
                var name = string.Empty;
                if (TagsList.Count() > 0)
                {
                    var query = TagsList.Where(p => p.TagsId == TagsId);
                    name = query != null ? query.FirstOrDefault().TagsName : "";
                }
                return name;
            }
        }
        /// <summary>
        /// 分类列表
        /// </summary>
        public virtual List<TagsModel> TagsList { get; set; } = new List<TagsModel>();
    }
}
