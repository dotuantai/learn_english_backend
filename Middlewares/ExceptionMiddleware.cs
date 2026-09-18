using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace learn_english_backend.Middlewares;

public sealed class ExceptionMiddleware(
    RequestDelegate next,
    ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
        {
            // The client disconnected; there is no response to deliver.
            context.Abort();
        }
        catch (Exception exception)
        {
            var traceId = Activity.Current?.Id ?? context.TraceIdentifier;
            logger.LogError(exception, "Unhandled request error. TraceId: {TraceId}", traceId);

            if (context.Response.HasStarted)
            {
                throw;
            }

            context.Response.Clear();
            var statusCode = exception is BadHttpRequestException badRequest
                ? badRequest.StatusCode
                : StatusCodes.Status500InternalServerError;

            context.Response.StatusCode = statusCode;

            var problem = new ProblemDetails
            {
                Status = statusCode,
                Title = statusCode >= 500
                    ? "An unexpected error occurred."
                    : "The request could not be processed.",
                Extensions = { ["traceId"] = traceId }
            };

            await context.Response.WriteAsJsonAsync(
                problem,
                options: null,
                contentType: "application/problem+json",
                cancellationToken: context.RequestAborted);
        }
    }
}
