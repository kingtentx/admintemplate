using King.Data.ExtModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace King.Data
{
    /// <summary>
    /// 相册
    /// </summary>
    public class Album : ExtFullModifyModel, IActiveModel, IModifyModel
    {
        [Key]
        public int AlbumId { get; set; }       
        /// <summary>
        /// 图片
        /// </summary>
        [StringLength(ModelUnits.Len_500)]
        public string ImageUrl { get; set; }
        /// <summary>
        /// 标题
        /// </summary>      
        [StringLength(ModelUnits.Len_250)]
        public string Title { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [StringLength(ModelUnits.Len_500)]
        public string Description { get; set; }
        /// <summary>
        /// 作者
        /// </summary>
        [StringLength(ModelUnits.Len_50)]
        public string Author { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public int TagsType { get; set; }
        /// <summary>
        /// 标签ID
        /// </summary>
        public int TagsId { get; set; }
        /// <summary>
        /// 是否公开
        /// </summary>
        public bool IsActive { get ; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; }
    }
}
