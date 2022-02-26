using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace King.AdminSite.Models
{
    public class AlbumOutput
    {
        /// <summary>
        /// ID
        /// </summary>
        public int AlbumId { get; set; }      
        /// <summary>
        /// 图片
        /// </summary>
        public string ImageUrl { get; set; }
        /// <summary>
        /// 标题
        /// </summary>      
        public string Title { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }       
       

        public string CreateTime { get; set; }

        
    }
}
