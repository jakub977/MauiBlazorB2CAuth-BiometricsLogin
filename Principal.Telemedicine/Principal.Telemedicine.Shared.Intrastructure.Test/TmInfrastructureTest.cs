using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Principal.Telemedicine.Shared.Intrastructure;
using Moq;
using Principal.Telemedicine.Shared.Infrastructure;
using Principal.Telemedicine.Shared.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Principal.Telemedicine.Shared.Contants;
using Principal.Telemedicine.DataConnectors.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using Principal.Telemedicine.Shared.Logging;
using Microsoft.AspNetCore.Hosting;

namespace Principal.Telemedicine.Shared.Intrastructure.Test;

public class TmInfrastructureTest
{
    private DependencyResolverHelper serviceProvider;

    public TmInfrastructureTest()
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
                        services.AddDbContext<DbContextGeneral>(fn =>
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
                        services.AddLogging();

                    })

                    .Configure(app =>
                    {
                        app.UseMiddleware<LoggingMiddleware>();
                    });

            });

        serviceProvider = new DependencyResolverHelper(hostBuilder.StartAsync().Result);
    }

    [Fact]
    public void Middleware_ShouldAddHeaderToCache()
    {
       
      
    }



    [Fact(DisplayName = "Test konfigurace prostředí aplikace")]
    public void TestConfiguration()
    {
        var config = serviceProvider.GetService<IOptions<TmAppConfiguration>>();
        Assert.NotNull(config);
        Assert.Equal("-", config.Value.IdentificationDelimeter);
        Assert.Equal("testApi", config.Value.IdentificationId, true );
    }
}