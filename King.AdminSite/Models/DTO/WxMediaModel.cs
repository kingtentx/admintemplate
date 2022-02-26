using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace King.AdminSite.Models
{
    public class WxMediaModel
    {
        public long MeId { get; set; }
        
        /// <summary>
        /// 图片名称
        /// </summary>

        public string FileName { get; set; }
        /// <summary>
        /// 介绍
        /// </summary>

        public string Introduction { get; set; }
        /// <summary>
        /// 新增的永久素材的media_id
        /// </summary>

        public string MediaId { get; set; }
        /// <summary>
        /// Type: news=1 image=2 voice=3 video=4 
        /// </summary>
        public int MediaType { get; set; }
        /// <summary>
        /// 本地服务器URL路径
        /// </summary>

        public string Url { get; set; }
        /// <summary>
        /// 封面
        /// </summary>

        public string CoverUrl { get; set; }
        /// <summary>
        /// 扩展名
        /// </summary>

        public string ExtensionName { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public long Size { get; set; }
        /// <summary>
        /// 标签ID
        /// </summary>       
        public int TagId { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public DateTime? CreateTime { get; set; }
        public string CreateBy { get; set; }
        public bool IsDelete { get; set; }
    }
}
