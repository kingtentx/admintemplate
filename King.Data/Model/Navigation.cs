using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using King.Data.ExtModel;

namespace King.Data
{
    /// <summary>
    /// 导航
    /// </summary> 

    public class Navigation : ExtFullModifyModel, IActiveModel, ISortModel, IModifyModel
    {
        /// <summary>
        /// 分类ID
        /// </summary>
        [Key]
        public int NavigationId { get; set; }
        /// <summary>
        /// 父ID
        /// </summary>
        public int ParentId { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        [StringLength(ModelUnits.Len_50)]
        public string NavigationName { get; set; }
        /// <summary>
        /// 重定向名称
        /// </summary>
        [StringLength(ModelUnits.Len_50)]
        public string RewriteName { get; set; }       
        /// <summary>
        /// 描述
        /// </summary>
        [StringLength(ModelUnits.Len_500)]
        public string Description { get; set; }
        /// <summary>
        /// 指定跳转链接
        /// </summary>
        [StringLength(ModelUnits.Len_500)]
        public string LinkUrl { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 是否主页
        /// </summary>
        public bool IsHomePage { get; set; }
        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsShow { get; set; }
        /// <summary>
        /// 是否激活
        /// </summary>
        public bool IsActive { get; set; }

        public bool IsDelete { get; set; }

    }
}
