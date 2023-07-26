using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Principal.Telemedicine.DataConnectors.Models;


namespace Principal.Telemedicine.Shared.Logging.Tests
{
    public class TestStartup 
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json")
          .Build();
        
            services.AddDbContext<DbContextGeneral>(fn => fn.UseInMemoryDatabase(databaseName: "TestDatabaseLogCustom"));
        
            services.AddLogging(configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Zde se použije testovaný Middleware
            app.UseMiddleware<LoggingMiddleware>();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
