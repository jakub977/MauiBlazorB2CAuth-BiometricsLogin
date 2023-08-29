using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Principal.Telemedicine.DataConnectors.Contexts;
using Principal.Telemedicine.DataConnectors.Models.General;

namespace Principal.Telemedicine.Shared.Logging;
/// <summary>
/// Custom ILogger pro logování do DB
/// </summary>
public class TelemedicineDbLogger:ILogger
{
    private readonly string _categoryName;
    private readonly string? _connString;
    private IServiceProvider _serviceProvider;


    //public TelemedicineDbLogger(string categoryName,string? connString)
    //{
    //    _categoryName = categoryName;
    //    _connString = connString;
    //}

    public TelemedicineDbLogger(string categoryName, IServiceProvider serviceProvider)
    {
        _categoryName = categoryName;
        _serviceProvider = serviceProvider;
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
            logEntry = new() { FullMessage = message, ShortMessage = message.Substring(0,message.Length>4000?4000:message.Length), CreatedDateUtc = DateTime.UtcNow, Source = Environment.ProcessPath.Substring(0, Environment.ProcessPath.Length>99?99:Environment.ProcessPath.Length), FriendlyTopic = logLevel.ToString()  };
        }
        try
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _dbContextGeneral = scope.ServiceProvider.GetRequiredService<DbContextGeneral>();

                if (_dbContextGeneral != null)
                {
                    _dbContextGeneral.Logs.Add(logEntry);
                    _dbContextGeneral.Add(logEntry);
                    _dbContextGeneral.SaveChanges();
                }
                else
                {
                    if (logLevel == LogLevel.None) throw new Exception("Logging audit record could not be created, DbContext is null");
                }
            }
        }
        catch(Exception ex)
        {
            if (logLevel == LogLevel.None) throw new Exception("Logging audit record failed " + ex.ToString());
        }
    }
}

