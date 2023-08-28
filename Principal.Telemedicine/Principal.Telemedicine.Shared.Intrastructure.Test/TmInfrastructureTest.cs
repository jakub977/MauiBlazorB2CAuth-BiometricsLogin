﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Principal.Telemedicine.DataConnectors.Models;
using Principal.Telemedicine.Shared.Configuration;
using Principal.Telemedicine.Shared.Constants;
using Principal.Telemedicine.Shared.Infrastructure;
using Principal.Telemedicine.Shared.Logging;

namespace Principal.Telemedicine.Shared.Intrastructure.Test;

public class TmInfrastructureTest
{
    private DependencyResolverHelper serviceProvider;
    private IHost host;
    private const string HEADER_PUV = "API_TEST_HEADER";
    public TmInfrastructureTest()
    {
        var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json")
            .Build();
             var hostBuilder = new HostBuilder()
            .ConfigureWebHost(webBuilder =>
            {
                webBuilder
                   .UseTestServer().UseEnvironment("local")
                    .ConfigureServices(services =>
                    {
                        services.AddMemoryCache();
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
                        services.AddSecretConfiguration<TmAppConfiguration>(configuration,"/secrets.json");
                        services.AddTmInfrastructure(configuration);

                    })

                    .Configure(app =>
                    {
                        app.UseMiddleware<TracingMiddleware>();
                        app.UseMiddleware<LoggingMiddleware>();
                    });

            });
        host = hostBuilder.StartAsync().Result;
        serviceProvider = new DependencyResolverHelper(host);
    }

    [Fact(DisplayName ="Testing Middleware function")]
    public void Middleware_ShouldAddHeaderToCache()
    {
        //Arange
        var client = host.GetTestServer();//.GetAsync("/");
        client.BaseAddress = new Uri("https://contentserver/A/");
        client.CreateClient();
        
        var response = client.SendAsync(c => {
            c.Request.Method = HttpMethods.Get;
            c.Request.Path= "/";
            c.Request.Headers.Add(HeaderKeysConst.TRACE_KEY, HEADER_PUV);
        }).Result;

        var _cache = serviceProvider.GetService<IMemoryCache>();
        var _conf = serviceProvider.GetService<IOptions<TmAppConfiguration>>().Value;
        //Fakt
        Assert.NotNull(_cache);
        Assert.Equal($"{HEADER_PUV}{_conf.IdentificationDelimeter}{_conf.IdentificationId}", response.Response.Headers[HeaderKeysConst.TRACE_KEY].First(), true);
        
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