namespace Nimbus.Tests.WebApi
{
    using AutoFixture;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Options;
    using Nimbus.Business.Common;
    using Nimbus.WebApi.Middleware;
    using System.Threading.Tasks;
    using Xunit;

    public class ApiKeyMiddlewareTests
    {
        private readonly string _correctApiKey;
        private readonly string _invalidApiKey;
        public ApiKeyMiddlewareTests()
        {
            var fixture = new Fixture();
            _correctApiKey = fixture.Create<string>();
            _invalidApiKey = fixture.Create<string>();
        }
        [Fact]
        public async Task Should_ReturnUnauthorized_When_NoApiKeyIsPresent()
        {
            // Arrange
            var options = Options.Create(new AppSettings { ApiKey = _correctApiKey });
            var middleware = new ApiKeyMiddleware((innerHttpContext) => Task.CompletedTask, options);

            var context = new DefaultHttpContext();
            context.Request.Headers["X-API-KEY"] = string.Empty;

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            Assert.Equal(401, context.Response.StatusCode);
        }

        [Fact]
        public async Task Should_ReturnUnauthorized_When_InvalidApiKeyIsProvided()
        {
            // Arrange
            var options = Options.Create(new AppSettings { ApiKey = _correctApiKey });
            var middleware = new ApiKeyMiddleware((innerHttpContext) => Task.CompletedTask, options);

            var context = new DefaultHttpContext();
            context.Request.Headers["X-API-KEY"] = _invalidApiKey;

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            Assert.Equal(401, context.Response.StatusCode);
        }

        [Fact]
        public async Task Should_CallNextMiddleware_When_ValidApiKeyIsProvided()
        {
            // Arrange
            var options = Options.Create(new AppSettings { ApiKey = _correctApiKey });
            var middleware = new ApiKeyMiddleware((innerHttpContext) =>
            {
                innerHttpContext.Response.StatusCode = 200; // Simulating next middleware behavior
                return Task.CompletedTask;
            }, options);

            var context = new DefaultHttpContext();
            context.Request.Headers["X-API-KEY"] = _correctApiKey;

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            Assert.Equal(200, context.Response.StatusCode);
        }
    }

}
