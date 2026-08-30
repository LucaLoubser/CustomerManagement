using CustomerManagement.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using FluentValidation;

namespace CustomerManagement.Api.Middleware;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception ex, CancellationToken cancellationToken)
    {
        var (result, level) = ex switch
        {
            ValidationException v => (Results.ValidationProblem(
                v.Errors
                 .GroupBy(e => e.PropertyName)
                 .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())
            ), LogLevel.Information),
            CustomerNotFoundException => (Results.Problem(
                title: "Customer not found", detail: ex.Message,
                statusCode: StatusCodes.Status404NotFound
            ),LogLevel.Information),
            DuplicateEmailException => (Results.Problem(
                title: "Email already in use", detail: ex.Message,
                statusCode: StatusCodes.Status409Conflict
            ),LogLevel.Information),
            _ => (Results.Problem(
                title: "An unexpected error occurred",
                detail: "See server logs for details.",
                statusCode: StatusCodes.Status500InternalServerError
            ), LogLevel.Error)
        };

        logger.Log(level, ex, "Request failed {Method} {Path} {traceId}",
                context.Request.Method, context.Request.Path, context.TraceIdentifier);

        await result.ExecuteAsync(context);
        return true;
    }
}