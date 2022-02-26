using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace King.AdminSite.Models
{
    /// <summary>
    /// 站点信息
    /// </summary>
    public class SiteConfigModel
    {
        /// <summary>
        /// 
        /// </summary>      
        public int Id { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>     
        public string CompanyName { get; set; }
        /// <summary>
        /// SEO-Keywords
        /// </summary>      
        public string Keywords { get; set; }
        /// <summary>
        /// SEO-Description
        /// </summary>     
        public string Description { get; set; }
        /// <summary>
        /// Logo
        /// </summary>
        public string Logo { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>      
        public string Email { get; set; }
        /// <summary>
        /// 地址
        /// </summary>      
        public string Address { get; set; }
        /// <summary>
        /// 
        /// </summary>      
        public string Phone { get; set; }
        /// <summary>
        /// 备案号
        /// </summary>       
        public string RecordNo { get; set; }
        /// <summary>
        /// 版权
        /// </summary>      
        public string Copyright { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double Location_X { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double Location_Y { get; set; }

        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>          
        public string CreateBy { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// 更新人
        /// </summary>           
        public string UpdateBy { get; set; }

        public bool IsActive { get { return true; } } 
    }
}
