using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Principal.Telemedicine.DataConnectors.Models;
using Principal.Telemedicine.Shared.Logging;
using Xunit;

namespace Principal.Telemedicine.Shared.Logging.Tests
{
    public class DiRegistrationTests
    {
        [Fact]
        public void AddLogging_Should_Register_CompositeLoggerProvider()
        {
            var options = new DbContextOptionsBuilder<DbContextGeneral>()
           .UseInMemoryDatabase(databaseName: "TestDatabase")
           .Options;
            // Arrange
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json")
                .Build();

            // Registr logging
            var hostBuilder = new HostBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddDbContext<DbContextGeneral>(fn=>fn.UseInMemoryDatabase(databaseName: "TestDatabase"));
                    // Register the DiRegistration class
                    services.AddLogging(configuration);
                });

            // Act
            var host = hostBuilder.Build();
            var serviceProvider = host.Services;
            var loggerProvider = serviceProvider.GetRequiredService<ILoggerProvider>();

            // Assert
            Assert.IsType<CompositeLoggerProvider>(loggerProvider);

            ILogger logger = loggerProvider.CreateLogger("UnitTest");
            logger.LogInformation("Test");

            using var _context = (DbContextGeneral) serviceProvider.GetServices(typeof(DbContextGeneral));
            Assert.NotNull(_context.Logs.FirstOrDefault());
            

        }

     
    }
}
