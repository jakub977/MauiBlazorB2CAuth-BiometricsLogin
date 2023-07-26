using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Principal.Telemedicine.Shared.Logging.Tests
{
    public class LoggingMiddlewareTests : IClassFixture<WebApplicationFactory<TestStartup>>
    {
        private readonly WebApplicationFactory<TestStartup> _factory;

        public LoggingMiddlewareTests(WebApplicationFactory<TestStartup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task LoggingMiddleware_Should_Log_Request_And_Response()
        {
            // Arrange
            var client = _factory.CreateClient();
            var loggerMock = new Mock<ILogger<LoggingMiddleware>>();
            var middleware = new LoggingMiddleware(next: (innerHttpContext) => Task.FromResult(0), logger: loggerMock.Object);

            var requestContent = "Sample request body";
            var request = new HttpRequestMessage(HttpMethod.Post, "/test")
            {
                Content = new StringContent(requestContent, System.Text.Encoding.UTF8, "application/json")
            };
            request.Headers.Add("Custom-Header", "CustomHeaderValue");

            // Act
            var response = await client.SendAsync(request);

            // Assert            
            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString().Contains("=== Request Info ===") && o.ToString().Contains("method = POST") && o.ToString().Contains("path = /test")),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()
                ),
                Times.Once
            );

            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString().Contains("=== Response Info ===") && o.ToString().Contains("Status code = 200") && o.ToString().Contains("Content-Type = application/json")),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()
                ),
                Times.Once
            );

         
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);
        }
    }
}
