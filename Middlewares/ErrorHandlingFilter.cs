using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TasikmalayaKota.Simpatik.Web.Middlewares
{
    public class ErrorHandlingFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            context.ExceptionHandled = true;
            context.Result = new ViewResult
            {
                ViewName = "~/Views/Error/HandlePageNotFound.cshtml"
            };
        }
    }
}
