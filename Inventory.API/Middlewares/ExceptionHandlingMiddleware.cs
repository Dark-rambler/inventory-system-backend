using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Middlewares
{
    public class ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        private static readonly Dictionary<Type, (int StatusCode, string Title)> ExceptionMap = new()
        {
            [typeof(KeyNotFoundException)] = (StatusCodes.Status404NotFound, "Resource Not Found"),
            [typeof(UnauthorizedAccessException)] = (StatusCodes.Status401Unauthorized, "Unauthorized"),
            [typeof(ArgumentException)] = (StatusCodes.Status400BadRequest, "Bad Request"),
            [typeof(InvalidOperationException)] = (StatusCodes.Status400BadRequest, "Bad Request"),
        };

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (ValidationException validationException)
            {
                logger.LogError("Validation error: {Message}", validationException.Message);
                await WriteValidationErrorAsync(context, validationException);
            }
            catch (Exception ex)
            {
                logger.LogError("Exception occurred: {Message}", ex.Message);

                if (ExceptionMap.TryGetValue(ex.GetType(), out var mapped))
                    await WriteProblemAsync(context, mapped.StatusCode, mapped.Title, ex.Message);
                else
                    await WriteProblemAsync(context, StatusCodes.Status500InternalServerError, "Server Error", ex.Message);
            }
        }

        private static Task WriteValidationErrorAsync(HttpContext context, ValidationException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            var problem = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation Error",
                Detail = "One or more validation errors occurred.",
            };
            problem.Extensions["errors"] = ex.Errors.Select(e => new { field = e.PropertyName, error = e.ErrorMessage });
            return context.Response.WriteAsJsonAsync(problem);
        }

        private static Task WriteProblemAsync(HttpContext context, int statusCode, string title, string detail)
        {
            context.Response.StatusCode = statusCode;
            return context.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail,
            });
        }
    }
}
