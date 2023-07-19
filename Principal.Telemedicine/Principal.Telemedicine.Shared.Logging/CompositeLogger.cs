using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Principal.Telemedicine.Shared.Logging
{
    public class CompositeLogger : ILogger
    {
        private readonly List<ILogger> _loggers;

        public CompositeLogger(List<ILogger> loggers)
        {
            _loggers = loggers;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            // Pro CompositeLogger záleží na jednotlivých logerech, jestli jsou zapnuty nebo ne
            foreach (var logger in _loggers)
            {
                if (logger.IsEnabled(logLevel))
                {
                    return true;
                }
            }

            return false;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            foreach (var logger in _loggers)
            {
                if (logger.IsEnabled(logLevel))
                {
                    logger.Log(logLevel, eventId, state, exception, formatter);
                }
            }
        }
    }
}
