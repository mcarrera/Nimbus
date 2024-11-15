using System.Net;
namespace Nimbus.WebApi.Middleware;

public class GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred");
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            // Optionally, send a custom error message in the response body
            await httpContext.Response.WriteAsync("An error occurred while processing your request.");
        }
    }
}
