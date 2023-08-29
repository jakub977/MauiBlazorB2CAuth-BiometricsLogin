using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Principal.Telemedicine.DataConnectors.Contexts;
using Xunit;

namespace Principal.Telemedicine.Shared.Logging.Tests
{
    public class TelemedicineDbLoggerTests
    {
        [Fact(DisplayName ="Test custom logeru pro zápis do databáze")]
        public void TelemedicineDbLogger_Should_Log_To_Database_When_Enabled()
        {

            var services = new ServiceCollection();

           
            services.AddDbContext<DbContextGeneral>(options =>
            {             
                options.UseInMemoryDatabase( databaseName: "TestDatabaseDbTest");
            });
            var _serviceProvider = services.BuildServiceProvider();

            string loggedMessage = null;
            var telemedicineDbLogger = new TelemedicineDbLogger("TestCategory", _serviceProvider);

            // Act
            telemedicineDbLogger.Log(LogLevel.Information, new EventId(), "Test message", null, (s, e) => s.ToString());


            using (var context = _serviceProvider.GetRequiredService<DbContextGeneral>())
            {

                // Assert
                var logEntry = context.Logs.FirstOrDefault();
                Assert.NotNull(logEntry);
                Assert.Contains("Test message", logEntry.FullMessage); // Je logovaná zpráva součástí db?
            }
           
            
        }
    }
}
