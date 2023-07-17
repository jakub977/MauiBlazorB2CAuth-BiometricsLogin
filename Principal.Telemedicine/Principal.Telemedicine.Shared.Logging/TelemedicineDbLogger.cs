using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Principal.Telemedicine.DataConnectors.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
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

        if (exception != null)
        {
            var logEntry = new Log
            {
              //...
                CreatedDateUtc = DateTime.UtcNow
            };

            // Uložení logovacího záznamu do databáze
            _context.Logs.Add(logEntry);

        }
        else
        {
            var logEntry = new Log()
            {
             //.....
                CreatedDateUtc = DateTime.UtcNow
            };

            // Uložení logovacího záznamu do databáze
            _context.Add(logEntry);
        }
        _context.SaveChanges();
    }
}

