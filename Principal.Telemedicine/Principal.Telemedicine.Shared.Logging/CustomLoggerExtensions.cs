using Microsoft.Extensions.Logging;
using Principal.Telemedicine.Shared.Logging.Enumerators;
using System;
using Principal.Telemedicine.DataConnectors.Models;
using System.Text.Json;

namespace Principal.Telemedicine.Shared.Logging;
public static class CustomLoggerExtensions
{
    public static void LogCustom<T>(this ILogger<T> logger, CustomLogLevel customLogLevel, string friendlyTopic, string source, string shortMessage,  string fullMessage, string aditionalInfo, string internalComunicationId )
    {
        LogLevel logLevel = MapCustomLogLevelToLogLevel(customLogLevel);
        Log logEntry = new() {Source= source, AdditionalInfo = aditionalInfo, CreatedDateUtc=DateTime.UtcNow, Environment= Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") , InternalCommunicationId= internalComunicationId, FriendlyTopic = friendlyTopic, FullMessage = fullMessage, ShortMessage = shortMessage  };
        
        string message = JsonSerializer.Serialize(logEntry);
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
