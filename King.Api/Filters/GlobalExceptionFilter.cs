
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace King.Api.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            Log.Error(context.Exception, "GlobalExceptionFilter");
        }
    }
}
