using King.Api.Models;
using King.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace King.Api
{
    public class Utils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Claims"></param>
        /// <returns></returns>
        public static LoginUser LoginUser(ClaimsPrincipal Claims)
        {
            var identity = (ClaimsIdentity)Claims.Identity;

            LoginUser user = new LoginUser()
            {
                UserId = Convert.ToInt32(identity.FindFirst(ClaimTypes.Sid)?.Value),
                UserName = identity.FindFirst(ClaimTypes.Name)?.Value
            };

            return user;
        }

        /// <summary>
        /// 身份证验证
        /// </summary>
        /// <param name="idCard"></param>
        /// <returns></returns>
        public static bool IsIdCard(string idCard)
        {
            switch (idCard.Length)
            {
                case 15:
                    return Regex.IsMatch(idCard, @"^[1-9]\d{7}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}$");
                case 18:
                    return Regex.IsMatch(idCard, @"^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])((\d{4})|\d{3}[A-Z])$", RegexOptions.IgnoreCase);
                default:
                    return false;
            }
        }
    }
}
