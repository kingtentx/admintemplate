using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using King.Data.ExtModel;

namespace King.Data
{
    /// <summary>
    /// 角色
    /// </summary>
    public class Role:ExtCreateModel, IActiveModel
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        [Key]
        public int RoleId { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        [StringLength(ModelUnits.Len_50)]
        public string RoleName { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [StringLength(ModelUnits.Len_500)]
        public string Description { get; set; }
       
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsActive { get; set; }
    }
}