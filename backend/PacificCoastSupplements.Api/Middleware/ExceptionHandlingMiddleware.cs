using Microsoft.AspNetCore.Mvc;
using PacificCoastSupplements.Api.Exceptions;

namespace PacificCoastSupplements.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");

                var (statusCode, title) = ex switch
                {
                    BadRequestException => (StatusCodes.Status400BadRequest, "Bad Request"),
                    ArgumentException => (StatusCodes.Status400BadRequest, "Bad Request"),
                    NotFoundException => (StatusCodes.Status404NotFound, "Not Found"),
                    KeyNotFoundException => (StatusCodes.Status404NotFound, "Not Found"),
                    _ => (StatusCodes.Status500InternalServerError, "Internal Server Error")
                };

                var problem = new ProblemDetails
                {
                    Status = statusCode,
                    Title = title,
                    Detail = ex.Message,
                    Type = statusCode switch
                    {
                        400 => "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                        404 => "https://tools.ietf.org/html/rfc9110#section-15.5.5",
                        _ => "https://tools.ietf.org/html/rfc9110#section-15.6.1"
                    }
                };

                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/problem+json";

                await context.Response.WriteAsJsonAsync(problem);
            }
        }
    }
}
