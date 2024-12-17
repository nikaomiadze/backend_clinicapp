using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using clinic.packpages;

namespace clinic.filters
{
    public class GlobalExceptionFilter : ExceptionFilterAttribute
    {
        readonly IPKG_LOG logs;

        public GlobalExceptionFilter(IPKG_LOG logs)
        {
            this.logs = logs;
        }

        public override void OnException(ExceptionContext context)
        {
            var result = new ObjectResult("System Error. Try Again")
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };

            var error = context.Exception.Message;

            try
            {
                logs.Add_log(error);
            }
            catch
            {
            }

            context.Result = result;
        }
    }
}
