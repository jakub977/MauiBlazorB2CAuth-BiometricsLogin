using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Principal.Telemedicine.Shared.Logging.Tests
{
    public class DiRegistrationTests
    {
        [Fact]
        public void AddLogging_Should_Register_CompositeLoggerProvider()
        {
            // Arrange
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json")
                .Build();

            // Registr logging
            var hostBuilder = new HostBuilder()
                .ConfigureServices((context, services) =>
                {
                   
                    // Register the DiRegistration class
                    services.AddLogging(configuration);
                });

            // Act
            var host = hostBuilder.Build();
            var serviceProvider = host.Services;
            var loggerProvider = serviceProvider.GetRequiredService<ILoggerProvider>();

            // Assert
            Assert.IsType<CompositeLoggerProvider>(loggerProvider);
        }

     
    }
}
