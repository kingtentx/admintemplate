using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace King.AdminSite.Models
{
    public class PictureGalleryModel
    {
        public long Id { get; set; }
        public string ImageName { get; set; }
        /// <summary>
        /// 路径
        /// </summary>    
        public string Url { get; set; }
        /// <summary>
        /// 缩略图路径
        /// </summary>      
        public string ThumbnailUrl { get; set; }
        /// <summary>
        /// 扩展名
        /// </summary>       
        public string ExtensionName { get; set; }
        /// <summary>
        /// MD5值
        /// </summary>       
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
        public string CreateBy { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}
