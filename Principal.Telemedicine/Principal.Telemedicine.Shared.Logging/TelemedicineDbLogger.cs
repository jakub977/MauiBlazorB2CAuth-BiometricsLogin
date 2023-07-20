using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Principal.Telemedicine.DataConnectors.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Principal.Telemedicine.Shared.Logging;
/// <summary>
/// Custom ILogger pro logování do DB
/// </summary>
public class TelemedicineDbLogger:ILogger
{
    private readonly string _categoryName;
    private readonly DbContextGeneral _context;

    public TelemedicineDbLogger(string categoryName, DbContextGeneral context)
    {
        _categoryName = categoryName;
       _context = context;
    }

    /// <summary>
    /// Implementace interface
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    /// <param name="state"></param>
    /// <returns></returns>
    public IDisposable BeginScope<TState>(TState state)
    {
        return null;
    }

    /// <summary>
    /// Defaultní hodnota pro IsEnabled - vše, co je víc, nebo rovno Information
    /// </summary>
    /// <param name="logLevel"></param>
    /// <returns></returns>
    public bool IsEnabled(LogLevel logLevel)
    {
      
        if (logLevel == LogLevel.None)
        {
            return true;
        }


        return logLevel >= LogLevel.Information;
    }
    /// <summary>
    /// Metoda pro zápis logu do databáze
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    /// <param name="logLevel"></param>
    /// <param name="eventId"></param>
    /// <param name="state"></param>
    /// <param name="exception"></param>
    /// <param name="formatter"></param>
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }
        
        var message = formatter(state, exception);

        if (string.IsNullOrEmpty(message) && exception == null)
        {
            return;
        }
        Log? logEntry = null;

        try
        {
            logEntry = JsonSerializer.Deserialize<Log>(message);
        }
        catch // Není JSON nebo je jiný než očekávaný
        {
            logEntry = new() { FullMessage = message, ShortMessage = message.Substring(0,message.Length>4000?4000:message.Length), CreatedDateUtc = DateTime.UtcNow, Source = Environment.ProcessPath, FriendlyTopic = logLevel.ToString()  };
        }
        
            _context.Logs.Add(logEntry);
            _context.Add(logEntry);
            _context.SaveChanges();
        
    }
}

