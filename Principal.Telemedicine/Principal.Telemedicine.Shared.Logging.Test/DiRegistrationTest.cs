using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Principal.Telemedicine.DataConnectors.Models;
using Principal.Telemedicine.Shared.Logging.Test.Helpers;
using Xunit;

namespace Principal.Telemedicine.Shared.Logging.Tests;


public class DiRegistrationTests
{
    private DependencyResolverHelper serviceProvider;
    public DiRegistrationTests()
    {
        // Arrange
        var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json")
            .Build();

        // Registr logging
        var hostBuilder = new HostBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddDbContext<DbContextGeneral>(fn =>
                {
                    fn.UseInMemoryDatabase(databaseName: "TestDatabaseLogCustom");
                    fn.EnableSensitiveDataLogging(false);
                    var loggerFactory = LoggerFactory.Create(builder =>
                    {
                        builder.AddFilter((category, level) => false);
                    });
                    fn.UseLoggerFactory(loggerFactory);
                });
                // Register the DiRegistration class
                services.AddLogging(configuration);
                DiRegistration.AddLogging(services, configuration);

            });

        serviceProvider = new DependencyResolverHelper(hostBuilder.Build());
    }

    [Fact(DisplayName ="Test Registrace Logování a odzkoušení extenze se zápisem do databáze")]
    public void AddLogging_Test_CompositeLoggerProvider()
    {

        // Act
        var logger = serviceProvider.GetService<ILogger<DiRegistrationTests>>();
     
        logger.LogCustom(Enumerators.CustomLogLevel.Audit, "TestTopic", "this test source", "short message from log", "full message from log", "aditionalInfo from log", "idCommunication");
        
        using var _context = serviceProvider.GetService<DbContextGeneral>();
        var entity = _context?.Logs.FirstOrDefault(m=> m.Id==1);
        Assert.NotNull(entity);
        Assert.Equal("short message from log", entity?.ShortMessage);
       


        logger.LogInformation("Text simple message");
        entity = _context?.Logs.FirstOrDefault(m=> m.Id==2);
        Assert.NotNull(entity);
        Assert.Equal("Text simple message", entity?.ShortMessage);
        _context.Database.EnsureDeleted();

    }

 
}
