using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace King.Api.Models
{
    public class LoginUser
    {
        /// <summary>
        /// 
        /// </summary>
        public int UserId { get; set; } = 0;
        /// <summary>
        /// 管理员名称
        /// </summary>
        public string UserName { get; set; } = string.Empty;
       
    }
}
