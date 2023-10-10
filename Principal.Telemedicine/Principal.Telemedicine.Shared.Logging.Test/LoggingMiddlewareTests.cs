﻿using System.Net;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Principal.Telemedicine.DataConnectors.Contexts;
using Principal.Telemedicine.Shared.Logging.Test.Helpers;
using Xunit;

namespace Principal.Telemedicine.Shared.Logging.Tests
{
    public class LoggingMiddlewareTests 
    {
        private DependencyResolverHelper serviceProvider;


        public LoggingMiddlewareTests()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json")
             .Build();
            var hostBuilder = new HostBuilder()
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder
                       .UseTestServer()
                        .ConfigureServices(services =>
                        {
                            services.AddDbContext<DbContextApi>(fn =>
                            {
                                fn.UseInMemoryDatabase(databaseName: "TestDatabaseMiddleware");
                                fn.EnableSensitiveDataLogging(false);
                                var loggerFactory = LoggerFactory.Create(builder =>
                                {
                                    // Vypnutí logování pro všechny kategorie
                                    builder.AddFilter((category, level) => false);
                                });

                                fn.UseLoggerFactory(loggerFactory);
                            });
                            services.AddLogging(configuration);
                          
                        })

                        .Configure(app =>
                        {
                            app.UseMiddleware<LoggingMiddleware>();
                        });

                });

            serviceProvider = new DependencyResolverHelper(hostBuilder.StartAsync().Result);
        }

        [Fact(DisplayName = "Test Middleware pro zápis do logu")]
        public async Task LoggingMiddleware_Should_Log_Request_And_Response()
        {

            // Arrange

            var middleware = new LoggingMiddleware(next: (innerHttpContext) => Task.CompletedTask, serviceProvider.GetService<ILogger<LoggingMiddleware>>());

            var requestContent = "Sample request body";
            var request = new HttpRequestMessage(HttpMethod.Post, "/test")
            {
                Content = new StringContent(requestContent, Encoding.UTF8, "application/json")
            };
            request.Headers.Add("Custom-Header", "CustomHeaderValue");

            var responseContent = "Sample response body";
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(responseContent, Encoding.UTF8, "application/json")
            };

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers.Add("Custom-Header", "CustomHeaderValue");
            httpContext.Request.Method = "POST";
            httpContext.Request.Path = "/test";
            httpContext.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(requestContent));

            httpContext.Response.StatusCode = (int)response.StatusCode;
            httpContext.Response.Headers.Add("Content-Type", "application/json");
            httpContext.Response.Body = new MemoryStream(Encoding.UTF8.GetBytes(responseContent));

            // Act
            await middleware.Invoke(httpContext);

            // Assert
            var dbContext = serviceProvider.GetService<DbContextApi>();
            var logs = dbContext.Logs.ToList(); // Získáme logy z databáze

            // Ověříme, zda se zapsaly očekávané logy
            Assert.True(logs.Count >= 2); // Očekáváme alespoň 2 logy (jeden pro požadavek a jeden pro odpověď)
            Assert.Contains(logs, log => log.Logger.Contains("INPUT CALL REQUEST"));
            Assert.Contains(logs, log => log.HttpMethod.Contains("POST"));
            Assert.Contains(logs, log => log.Logger.Contains("INPUT CALL RESPONSE"));
        }

    }
}
