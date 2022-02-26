using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace King.AdminSite.Models
{
    public class RoleModel
    {
        /// <summary>
        /// 角色ID
        /// </summary>     
        public int RoleID { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>       
        public string RoleName { get; set; }

        /// <summary>
        /// 描述
        /// </summary>       
        public string Description { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsActive { get; set; }

        public List<ModuleModel> MenuList { get; set; }

        public string[] PermissionList { get; set; } = new string[] { };
    }
}
