using Contractor.Exceptions;
using Contractor.Tools;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Contractor.Middlewares
{
    public static class ExceptionMiddleware
    {
        public static void UseCustomExceptionHandler(this IApplicationBuilder app, ILogger logger)
        {
            app.UseExceptionHandler(appError => {
                appError.Run(async context => {

                    object errorResponse = null!;

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (contextFeature != null)
                    {
                        var exception = contextFeature.Error;
                        logger.LogError($"Something went wrong: {exception}");
                        int code;
                        switch (exception)
                        {
                            case EntityNotFoundException notFoundException:
                                code = (int)HttpStatusCode.NotFound;
                                errorResponse = ResponseDto.Create(HttpStatusCode.NotFound, ((HttpStatusCode)code).ToString())
                                .AddError(notFoundException.Message);
                                break;

                            case CustomValidationException validationException:
                                code = (int)HttpStatusCode.BadRequest;
                                errorResponse = ResponseDto.Create(HttpStatusCode.BadRequest, ((HttpStatusCode)code).ToString())
                                .AddError(exception.Message, validationException.Property);
                                code = (int)HttpStatusCode.BadRequest;
                                break;

                            case BusinessRuleException businessRuleException:
                                code = (int)HttpStatusCode.BadRequest;
                                errorResponse = ResponseDto.Create(HttpStatusCode.BadRequest, ((HttpStatusCode)code).ToString())
                                .AddError(businessRuleException.Message);
                                code = (int)HttpStatusCode.BadRequest;
                                break;

                            case ValidationException validationException:
                                code = (int)HttpStatusCode.BadRequest;
                                errorResponse = ResponseDto.Create(HttpStatusCode.BadRequest, ((HttpStatusCode)code).ToString())
                                .AddError(validationException.Message);
                                code = (int)HttpStatusCode.BadRequest;
                                break;

                            case DbUpdateConcurrencyException dbUpdateConcurrencyException:
                                code = (int)HttpStatusCode.BadRequest;
                                errorResponse = ResponseDto.Create(HttpStatusCode.BadRequest, ((HttpStatusCode)code).ToString())
                                .AddError(dbUpdateConcurrencyException.Message);
                                code = (int)HttpStatusCode.BadRequest;
                                break;

                            case NotSupportedException notSupportedException:
                                code = (int)HttpStatusCode.InternalServerError;
                                errorResponse = ResponseDto.Create(HttpStatusCode.InternalServerError, ((HttpStatusCode)code).ToString())
                                .AddError(notSupportedException.Message);
                                code = (int)HttpStatusCode.InternalServerError;
                                break;

                            default:
                                code = (int)HttpStatusCode.InternalServerError;
                                errorResponse = ResponseDto.Create(HttpStatusCode.InternalServerError, ((HttpStatusCode)code).ToString())
                                .AddError(string.Format("Internal Error: {0}", exception.Message));
                                break;
                        }

                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = code;

                        var result = JsonConvert.SerializeObject(errorResponse);


                        await context.Response.WriteAsync(result);
                    }
                });
            });
        }
    }
}
