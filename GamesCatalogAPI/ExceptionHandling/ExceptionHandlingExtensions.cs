using EntityFramework.Exceptions.Common;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

using System.Net;

namespace Microsoft.Extensions.DependencyInjection;

public static class MyExceptionHandlingExtensions
{
    public static void MapExceptions(this WebApplication app)
    {
        app.UseExceptionHandler(
            options =>
            {
                options.Run(async context =>
                {
                    var feature = context.Features.Get<IExceptionHandlerPathFeature>();
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    var message = feature?.Error.ExceptionToString();
                    if (!string.IsNullOrEmpty(message))
                    {
                        var logger = context.RequestServices.GetService<ILogger<Program>>();
                        logger?.LogError("response error {message}", message);
                        await context.Response.WriteAsJsonAsync(new ProblemDetails()
                        {
                            Detail = message,
                            Status = (int)HttpStatusCode.InternalServerError,
                            Title = "Error"
                        });
                    }
                });
            }
        );
    }

    public static string ExceptionToString(this Exception ex) =>
        ex switch
        {
            BadHttpRequestException bre => $"{bre.Message.Replace("Failed to bind parameter \"Guid", " Cannot convert to parameter \"Guid")}",
            //BadRequestException br=> "Bad request",
            UniqueConstraintException uce => $"UniqueConstraintException",
            CannotInsertNullException cin => $"CannotInsertNullException",
            MaxLengthExceededException mle => $"MaxLengthExceededException",
            NumericOverflowException noe => "NumericOverflowException",
            ReferenceConstraintException noe => "ReferenceConstraintException",
            NotSupportedException ns => "Not supported",
            NotImplementedException ni => "Not imlemented",
            NullReferenceException nr => "Null reference",
            KeyNotFoundException knf => "Key not found",
            ArgumentNullException ane => $"{ane.ParamName} is null",
            ArgumentException ae => $"{ae.ParamName} & {ae.Message}",
            Exception are => $"{are.Message}",
            _ => "oops!"
        };
}

