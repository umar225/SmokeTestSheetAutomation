using Coursewise.Common.Exceptions;
using Coursewise.Common.Models;
using Coursewise.Logging;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Text;

namespace Coursewise.Api.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, ICoursewiseLogger<CoursewiseException> logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    var exceptionBuilder = new StringBuilder();
                    if (contextFeature != null)
                    {
                        if (contextFeature.Error.InnerException != null)
                        {
                            exceptionBuilder.Append($"Message {contextFeature.Error.InnerException.Message}");
                            exceptionBuilder.AppendLine();
                            exceptionBuilder.Append($"Stack trace {contextFeature.Error.InnerException.StackTrace}");
                        }
                        else
                        {
                            exceptionBuilder.Append($"Message {contextFeature.Error.Message}");
                            exceptionBuilder.AppendLine();
                            exceptionBuilder.Append($"Stack trace {contextFeature.Error.StackTrace}");
                        }
                        logger.Error(exceptionBuilder.ToString());
                        if (contextFeature.Error.GetType() == typeof(CoursewiseBaseException))
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            await context.Response.WriteAsync(BaseModel.Error(contextFeature.Error.Message).ToJsonString());
                        }
                        else
                        {
                            await context.Response.WriteAsync(BaseModel.Error("There was an error processing your request, please try again.").ToJsonString());
                        }

                    }
                });

            });
        }
    }

    public class CoursewiseException
    {

    }
}
