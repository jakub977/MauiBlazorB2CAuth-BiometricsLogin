using Microsoft.Extensions.Logging;
using Principal.Telemedicine.Shared.Logging.Enumerators;
using System.Text.Json;
using Principal.Telemedicine.DataConnectors.Models.Shared;
using Microsoft.AspNetCore.Http;

namespace Principal.Telemedicine.Shared.Logging;
/// <summary>
/// Extension pro ILogger pro přidání metody LogCustom
/// </summary>
public static class CustomLoggerExtensions
{
    /// <summary>
    /// Zaloguje genericky custom log do databáze
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="logger"></param>
    /// <param name="customLogLevel"></param>
    /// <param name="source"></param>
    /// <param name="shortMessage"></param>
    /// <param name="fullMessage"></param>
    /// <param name="aditionalInfo"></param>
    /// <param name="traceInformation"></param>
    /// <param name="httpMethod"></param>
    /// <param name="ipAddress"></param>
    public static void LogCustom<T>(this ILogger<T> logger, CustomLogLevel customLogLevel, string source, string shortMessage,  string fullMessage, string aditionalInfo, string traceInformation, string httpMethod = "" , string ipAddress = "" )
    {
        LogLevel logLevel = MapCustomLogLevelToLogLevel(customLogLevel);
        Log logEntry = new() {Logger= $"{typeof(T).Name} : {source}", ReferrerUrl = aditionalInfo, LogLevelId = (int) customLogLevel, CreatedOnUtc =DateTime.UtcNow, HttpMethod = httpMethod, FullMessage = fullMessage, ShortMessage = shortMessage, CorrelationGuid=traceInformation, IpAddress = ipAddress  };
        
        string message = JsonSerializer.Serialize(logEntry);
        logger.Log(logLevel, message);
    }

    /// <summary>
    ///  Zaloguje simple log do databáze
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="customLogLevel"></param>
    /// <param name="source"></param>
    /// <param name="shortMessage"></param>
    /// <param name="fullMessage"></param>
    /// <param name="aditionalInfo"></param>
    /// <param name="traceInformation"></param>
    /// <param name="httpMethod"></param>
    /// <param name="ipAddress"></param>
    public static void LogCustom(this ILogger logger, CustomLogLevel customLogLevel, string source, string shortMessage, string fullMessage, string aditionalInfo, string traceInformation, string httpMethod="", string ipAddress = "")
    {
        LogLevel logLevel = MapCustomLogLevelToLogLevel(customLogLevel);
        Log logEntry = new() { Logger = source, ReferrerUrl = aditionalInfo, LogLevelId = (int) customLogLevel, CreatedOnUtc = DateTime.UtcNow,  FullMessage = fullMessage, ShortMessage = shortMessage, CorrelationGuid = traceInformation, HttpMethod = httpMethod, IpAddress= ""  };

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
