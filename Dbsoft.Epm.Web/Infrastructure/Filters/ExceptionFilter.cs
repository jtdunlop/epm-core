using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Dbsoft.Epm.Web.Infrastructure.Filters
{
    using System.Security.Authentication;

    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is AuthenticationException)
            {
                context.Result = new RedirectResult("~/Account/Logout");
            }
        }

    
    }
}
