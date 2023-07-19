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
public class TelemedicineDbLogger:ILogger
{
    private readonly string _categoryName;
    private readonly DbContextGeneral _context;

    public TelemedicineDbLogger(string categoryName, DbContextGeneral context)
    {
        _categoryName = categoryName;
       _context = context;
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
      
        if (logLevel == LogLevel.None)
        {
            return true;
        }


        return logLevel >= LogLevel.Information;
    }

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
            logEntry = new() { FullMessage = message, CreatedDateUtc = DateTime.UtcNow, Source = Environment.ProcessPath, FriendlyTopic = logLevel.ToString()  };
        }
        
            _context.Logs.Add(logEntry);
            _context.Add(logEntry);
            _context.SaveChanges();
        
    }
}

