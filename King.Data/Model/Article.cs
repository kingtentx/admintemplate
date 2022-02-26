using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using King.Data.ExtModel;

namespace King.Data
{
    /// <summary>
    /// 文章
    /// </summary>
    public class Article : ExtFullModifyModel, IActiveModel, ISortModel, IModifyModel
    {
        [Key]
        public int ArticleId { get; set; }       
        
        /// <summary>
        /// 标题
        /// </summary>
        [Required]
        [StringLength(ModelUnits.Len_250)]
        public string Title { get; set; }
        /// <summary>
        /// 关键字
        /// </summary>
        [StringLength(ModelUnits.Len_250)]
        public string Keyword { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [StringLength(ModelUnits.Len_250)]
        public string Description { get; set; }
        /// <summary>
        /// 详情
        /// </summary>      
        public string Detail { get; set; }
        /// <summary>
        /// 作者
        /// </summary>
        [StringLength(ModelUnits.Len_50)]
        public string Author { get; set; }
        /// <summary>
        /// 来源
        /// </summary>
        [StringLength(ModelUnits.Len_100)]
        public string Source { get; set; }

        [StringLength(ModelUnits.Len_250)]
        public string SourceUrl { get; set; }
        /// <summary>
        /// 链接
        /// </summary>
        [StringLength(ModelUnits.Len_250)]
        public string LinkUrl { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        [StringLength(ModelUnits.Len_500)]
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
        [StringLength(ModelUnits.Len_100)]
        public string RecommendPosition { get; set; }
        /// <summary>
        /// 是否发表
        /// </summary>
        public bool IsActive { get; set; }

        public bool IsDelete { get; set; } = false;


    }
}
