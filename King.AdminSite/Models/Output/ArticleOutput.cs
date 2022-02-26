using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace King.AdminSite.Models
{
    public class ArticleOutput
    {
        public int ArticleId { get; set; }        
       
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
        
        /// <summary>
        /// 图片
        /// </summary>

        public string ImageUrl { get; set; } 
       

        public string CreateTime { get; set; }

        
    }
}
