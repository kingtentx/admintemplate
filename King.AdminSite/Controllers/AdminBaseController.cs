using log4net;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using King.AdminSite.Models;

namespace King.AdminSite.Controllers
{

    public class AdminBaseController : Controller
    {
        public ILog log;
        public AdminBaseController()
        {
            log = LogManager.GetLogger(Startup.logRepository.Name, this.GetType().Name);
        }

        /// <summary>
        /// 登录用户
        /// </summary>
        /// <param name="User"></param>
        /// <returns></returns>      
        public LoginUser LoginUser
        {
            get
            {
                var identity = (ClaimsIdentity)HttpContext.User.Identity;

                LoginUser user = new LoginUser()
                {
                    UserId = Convert.ToInt32(identity.FindFirst(ClaimTypes.Sid)?.Value),
                    UserName = identity.FindFirst(ClaimTypes.Name)?.Value,
                    Roles = identity.FindFirst(ClaimTypes.Role)?.Value,
                    IsAdmin = Convert.ToBoolean(identity.FindFirst(ClaimTypes.System)?.Value)
                };

                return user;
            }
        }
    }
}
