using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using King.Data.ExtModel;

namespace King.Data
{
    /// <summary>
    /// 管理员
    /// </summary>
    public class Admin : ExtUpdateModel, IActiveModel
    {
        /// <summary>
        /// 管理员ID
        /// </summary>
        [Key]
        public int AdminId { get; set; }
        /// <summary>
        /// 管理员名称
        /// </summary>
        [Required]
        [StringLength(ModelUnits.Len_50)]
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        [StringLength(ModelUnits.Len_50)]
        public string Password { get; set; }
        /// <summary>
        /// 手机
        /// </summary>      
        [StringLength(ModelUnits.Len_50)]
        public string Telphone { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>       
        [StringLength(ModelUnits.Len_50)]
        public string RealName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>       
        [StringLength(ModelUnits.Len_500)]
        public string Remark { get; set; }
        /// <summary>
        /// 角色
        /// </summary>       
        [StringLength(ModelUnits.Len_100)]
        public string Roles { get; set; }
        /// <summary>
        /// 是否超级管理员
        /// </summary>
        public bool IsAdmin { get; set; } = false;
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsActive { get; set; }

    }
}
