using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Aims.Filters
{
    /// <summary>
    /// 异常过滤器
    /// </summary>
    public class AimsExceptionFilter:ExceptionFilterAttribute
    {
        /// <summary>
        /// 重写
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task OnExceptionAsync(ExceptionContext context)
        {
            if (context.Exception is ValidationException exception)
            {
                var problemFactory = context.HttpContext.RequestServices.GetService<ProblemDetailsFactory>();
                object problem = problemFactory?.CreateProblemDetails(context.HttpContext, statusCode: (int)HttpStatusCode.BadRequest, title: exception.Message);
                context.Result = new BadRequestObjectResult(problem);
                context.ExceptionHandled = true;
            }
            return base.OnExceptionAsync(context);
        }
    }
}
