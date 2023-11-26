using Contractor.Tools;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Net;

namespace Contractor.Filters
{
    public class ValidateModelAttribute : IAsyncActionFilter
    {

        public ValidateModelAttribute()
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errorResponse = ResponseDto.Create(HttpStatusCode.BadRequest, "Fail")
                .AddModelStateErrors(context.ModelState, "ValidationFailed");

                context.HttpContext.Response.ContentType = "application/json";
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                var jsonResult = JsonConvert.SerializeObject(errorResponse);
                context.HttpContext.Response.WriteAsync(jsonResult).Wait();
                return;
            }
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // execute any code before the action executes here:
            if (!context.ModelState.IsValid)
            {
                var errorResponse = ResponseDto.Create(HttpStatusCode.BadRequest, "Fail")
                                          .AddModelStateErrors(context.ModelState, "ValidationFailed");

                context.HttpContext.Response.ContentType = "application/json";
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                var jsonResult = JsonConvert.SerializeObject(errorResponse);
                await context.HttpContext.Response.WriteAsync(jsonResult);
                return;
            }

            await next();
            // execute any code after the action executes here:
        }
    }
}
