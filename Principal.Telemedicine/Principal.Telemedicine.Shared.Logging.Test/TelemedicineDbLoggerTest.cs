using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Principal.Telemedicine.DataConnectors.Models;
using Xunit;

namespace Principal.Telemedicine.Shared.Logging.Tests
{
    public class TelemedicineDbLoggerTests
    {
        [Fact(DisplayName ="Test custom logeru pro zápis do databáze")]
        public void TelemedicineDbLogger_Should_Log_To_Database_When_Enabled()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DbContextGeneral>()
                .UseInMemoryDatabase(databaseName: "TestDatabaseDbLogger")
                .Options;

            using (var context = new DbContextGeneral(options))
            {
                string loggedMessage = null;
                var telemedicineDbLogger = new TelemedicineDbLogger("TestCategory", context);

                // Act
                telemedicineDbLogger.Log(LogLevel.Information, new EventId(), "Test message", null, (s, e) => s.ToString());

                // Assert
                var logEntry = context.Logs.FirstOrDefault();
                Assert.NotNull(logEntry);
                Assert.Contains("Test message", logEntry.FullMessage); // Je logovaná zpráva součástí db?

                context.Database.EnsureDeleted();
            }
            
        }
    }
}
