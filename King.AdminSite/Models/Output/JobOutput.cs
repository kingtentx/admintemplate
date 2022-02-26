using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace King.AdminSite.Models
{
    public class JobOutput
    {
        /// <summary>
        /// ID
        /// </summary>
        public int JobId { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string JobName { get; set; }       
        /// <summary>
        /// 描述
        /// </summary>
        public string Detail { get; set; }
        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }        
        /// <summary>
        /// 发布时间
        /// </summary>
        public string PublicTime { get; set; }


    }
}
