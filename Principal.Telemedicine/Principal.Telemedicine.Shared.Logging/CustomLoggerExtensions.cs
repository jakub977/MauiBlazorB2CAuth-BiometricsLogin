using Microsoft.Extensions.Logging;
using Principal.Telemedicine.Shared.Logging.Enumerators;
using System;

namespace Principal.Telemedicine.Shared.Logging;
public static class CustomLoggerExtensions
{
    public static void LogCustom<T>(this ILogger<T> logger, CustomLogLevel customLogLevel, string friendlyTopic, string source, string shortMessage,  string fullMessage, string aditionalInfo )
    {
        LogLevel logLevel = MapCustomLogLevelToLogLevel(customLogLevel);
        string message = $"{friendlyTopic.Replace("|", "-")}|{source.Replace("|","-")}|{shortMessage.Replace("|", "-")}|{fullMessage.Replace("|", "-")}|{aditionalInfo.Replace("|", "-")}";
        logger.Log(logLevel, message);
    }

    private static LogLevel MapCustomLogLevelToLogLevel(CustomLogLevel customLogLevel)
    {
        switch (customLogLevel)
        {
            case CustomLogLevel.Information:
                return LogLevel.Information;
            case CustomLogLevel.Warning:
                return LogLevel.Warning;
            case CustomLogLevel.Error:
                return LogLevel.Error;
            case CustomLogLevel.Audit:
                return LogLevel.None; 
            default:
                throw new ArgumentException($"Unknown custom log level: {customLogLevel}", nameof(customLogLevel));
        }
    }
}
