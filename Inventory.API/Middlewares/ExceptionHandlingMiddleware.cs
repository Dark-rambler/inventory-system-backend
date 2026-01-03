using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Middlewares
{
    public class ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception occurred: {Message}", ex.Message);

                var problemDetails = new ProblemDetails();

                switch (ex)
                {
                    case KeyNotFoundException:
                        problemDetails.Status = StatusCodes.Status404NotFound;
                        problemDetails.Title = "Resource Not Found";
                        problemDetails.Detail = ex.Message;
                        context.Response.StatusCode = StatusCodes.Status404NotFound;
                        break;

                    case UnauthorizedAccessException:
                        problemDetails.Status = StatusCodes.Status401Unauthorized;
                        problemDetails.Title = "Unauthorized";
                        problemDetails.Detail = ex.Message;
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        break;

                    case ArgumentException:
                        problemDetails.Status = StatusCodes.Status400BadRequest;
                        problemDetails.Title = "Bad Request";
                        problemDetails.Detail = ex.Message;
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        break;

                    default:
                        problemDetails.Status = StatusCodes.Status500InternalServerError;
                        problemDetails.Title = "Server Error";
                        problemDetails.Detail = ex.Message;
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        break;
                }

                await context.Response.WriteAsJsonAsync(problemDetails);
            }
        }

    }
}
