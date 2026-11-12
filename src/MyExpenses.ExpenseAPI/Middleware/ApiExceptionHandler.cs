using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyExpenses.Domain.Exceptions;

namespace MyExpenses.ExpenseAPI.Middleware;

internal sealed class ApiExceptionHandler(
    ILogger<ApiExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (statusCode, title, detail) = exception switch
        {
            DomainException => (
                StatusCodes.Status400BadRequest,
                "Invalid expense",
                "The request violates an expense business rule."),
            KeyNotFoundException => (
                StatusCodes.Status404NotFound,
                "Expense not found",
                "The requested expense could not be found."),
            _ => (0, null, null)
        };

        if (statusCode == 0)
        {
            return false;
        }

        logger.LogWarning(exception, "Handled API exception with status code {StatusCode}.", statusCode);

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(
            new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail,
                Instance = httpContext.Request.Path
            },
            cancellationToken);

        return true;
    }
}
