using King.Data.ExtModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace King.Data
{
    public class SiteConfig : ExtFullModifyModel,IActiveModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        [StringLength(ModelUnits.Len_100)]
        public string CompanyName { get; set; }
        /// <summary>
        /// SEO-Keywords
        /// </summary>
        [StringLength(ModelUnits.Len_250)]
        public string Keywords { get; set; }
        /// <summary>
        /// SEO-Description
        /// </summary>
        [StringLength(ModelUnits.Len_500)]
        public string Description { get; set; }
        /// <summary>
        /// Logo
        /// </summary>
        [StringLength(ModelUnits.Len_500)]
        public string Logo { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        [StringLength(ModelUnits.Len_250)]
        public string Email { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        [StringLength(ModelUnits.Len_500)]
        public string Address { get; set; }      
        /// <summary>
        /// 
        /// </summary>
        [StringLength(ModelUnits.Len_50)]
        public string Phone { get; set; }
        /// <summary>
        /// 备案号
        /// </summary>
        [StringLength(ModelUnits.Len_100)]
        public string RecordNo { get; set; }

        /// <summary>
        /// 版权
        /// </summary>
        [StringLength(ModelUnits.Len_250)]
        public string Copyright { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double Location_X { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double Location_Y { get; set; }
        public bool IsActive {  get; set; }
    }
}