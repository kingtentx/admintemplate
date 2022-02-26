using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using King.Data.ExtModel;

namespace King.Data
{
    /// <summary>
    /// 角色菜单
    /// </summary>
    public class RoleMenu : ExtCreateModel
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 角色ID
        /// </summary>
        public int RoleID { get; set; }       

        [StringLength(ModelUnits.Len_100)]
        public string Permission { get; set; }

    }
}