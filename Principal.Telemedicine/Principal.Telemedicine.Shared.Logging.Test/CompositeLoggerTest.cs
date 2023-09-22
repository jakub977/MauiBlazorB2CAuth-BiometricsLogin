using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Principal.Telemedicine.Shared.Logging.Tests;

public class CompositeLoggerTests
{
    [Fact(DisplayName = "Test kompozitního logovacího provideru")]
    public void Log_Should_Call_Log_Method_On_Enabled_Loggers()
    {
        // Arrange
        var mockLogger1 = new Mock<ILogger>();
        var mockLogger2 = new Mock<ILogger>();
        var mockLoggerProvider1 = new Mock<ILoggerProvider>();
        var mockLoggerProvider2 = new Mock<ILoggerProvider>();


        mockLoggerProvider1.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(mockLogger1.Object);
        mockLoggerProvider2.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(mockLogger2.Object);


        var compositeLoggerProvider = new CompositeLoggerProvider(new[] { mockLoggerProvider1.Object, mockLoggerProvider2.Object });
        var compositeLogger = compositeLoggerProvider.CreateLogger("TestCategory");


        mockLogger1.Setup(x => x.IsEnabled(It.IsAny<LogLevel>())).Returns(true);
        mockLogger2.Setup(x => x.IsEnabled(It.IsAny<LogLevel>())).Returns(true);

        // Act
        compositeLogger.Log(LogLevel.Information, new EventId(), "Test message", null, (s, e) => s.ToString());

        // Assert
        mockLogger1.Verify(x => x.Log(
            LogLevel.Information, It.IsAny<EventId>(), It.IsAny<string>(),
            It.IsAny<Exception>(), (Func<string, Exception, string>)It.IsAny<object>()
        ), Times.Once);

        mockLogger2.Verify(x => x.Log(
            LogLevel.Information, It.IsAny<EventId>(), It.IsAny<string>(),
            It.IsAny<Exception>(), (Func<string, Exception, string>)It.IsAny<object>()
        ), Times.Once);
    }
}
