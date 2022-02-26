using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace King.AdminSite.Models
{
    public class LoginViewModel
    {
        //用户名
        [Required]
        public string UserName { get; set; }
        //密码
        [Required]
        public string Password { get; set; }
        //key
        [Required]
        public string ValidateKey { get; set; }
        //验证码
        [Required]
        public string ValidateCode { get;set;}


    }
}
