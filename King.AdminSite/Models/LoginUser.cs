using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace King.AdminSite.Models
{
    public class LoginUser
    {
        public int UserId { get; set; } = 0;
        /// <summary>
        /// 管理员名称
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// 是否超级管理员
        /// </summary>
        public bool IsAdmin { get; set; } = false;

        /// <summary>
        /// 角色名称
        /// </summary>
        public string Roles { get; set; } = string.Empty;
    }
}
