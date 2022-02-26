using log4net;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace King.AdminSite.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private ILog log = LogManager.GetLogger(Startup.logRepository.Name, typeof(GlobalExceptionFilter));
        public void OnException(ExceptionContext context)
        {
            log.Error(context.Exception);
        }
    }
}
