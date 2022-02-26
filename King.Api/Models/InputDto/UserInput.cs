using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace King.Api.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class UserInput
    {
        /// <summary>
        /// 手机
        /// </summary>
        [Required]
        public string Telphone { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        public string Password { get; set; }
        /// <summary>
        /// key
        /// </summary>        
        public string ValidateKey { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>       
        public string ValidateValue { get; set; }
    }
}
