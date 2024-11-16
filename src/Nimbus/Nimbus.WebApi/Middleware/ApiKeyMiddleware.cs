using Microsoft.Extensions.Options;
using Nimbus.Business.Common;
using System.Net;

namespace Nimbus.WebApi.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _apiKey;

        public ApiKeyMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
        {
            _next = next;
            ArgumentException.ThrowIfNullOrWhiteSpace(appSettings.Value.ApiKey);
            _apiKey = appSettings.Value.ApiKey;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {

            if (!IsSwaggerRequest(httpContext.Request.Path))
            {
                // Check if the API key is valid and return a 401 error if it is not matching the one in the appsettings.json file
                var apiKey = httpContext.Request.Headers["X-API-KEY"].FirstOrDefault();

                if (string.IsNullOrEmpty(apiKey) || apiKey != _apiKey)
                {
                    httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    await httpContext.Response.WriteAsync("Unauthorized.");
                    return;
                }
            }

            await _next(httpContext);
        }

        private static bool IsSwaggerRequest(string path)
        {
            return path.Contains("/swagger", StringComparison.InvariantCultureIgnoreCase) ||
                   path.Contains("/swagger-ui", StringComparison.InvariantCultureIgnoreCase);
        }
    }

}
