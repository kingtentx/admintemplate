using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using King.Data.ExtModel;

namespace King.Data
{
    /// <summary>
    /// 图片库
    /// </summary>
    public class PictureGallery : ExtCreateModel, ICreateByModel
    {
        [Key]
        public long Id { get; set; }
        /// <summary>
        /// 图片名称
        /// </summary>
        [StringLength(ModelUnits.Len_100)]
        public string ImageName { get; set; }
        /// <summary>
        /// 路径
        /// </summary>
        [StringLength(ModelUnits.Len_500)]
        public string Url { get; set; }
        /// <summary>
        /// 缩略图路径
        /// </summary>
        [StringLength(ModelUnits.Len_500)]
        public string ThumbnailUrl { get; set; }
        /// <summary>
        /// 扩展名
        /// </summary>
        [StringLength(ModelUnits.Len_10)]
        public string ExtensionName { get; set; }
        /// <summary>
        /// MD5值
        /// </summary>
        [StringLength(ModelUnits.Len_100)]
        public string MD5 { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public long Size { get; set; }
        /// <summary>
        /// 原文件宽度
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// 原文件高度
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        [StringLength(ModelUnits.Len_100)]
        public string CreateBy { get ; set ; }
    }
}
