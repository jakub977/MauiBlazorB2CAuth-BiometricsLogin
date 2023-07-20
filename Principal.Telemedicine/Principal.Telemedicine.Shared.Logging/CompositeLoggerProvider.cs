using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Principal.Telemedicine.Shared.Logging
{
    public class CompositeLoggerProvider : ILoggerProvider
    {
        private readonly List<ILoggerProvider> _providers;

        public CompositeLoggerProvider(IEnumerable<ILoggerProvider> providers)
        {
            _providers = new List<ILoggerProvider>(providers);
        }

        public ILogger CreateLogger(string categoryName)
        {
            var loggers = new List<ILogger>();

            foreach (var provider in _providers)
            {
                loggers.Add(provider.CreateLogger(categoryName));
            }

            return new CompositeLogger(loggers);
        }

        public void Dispose()
        {
            foreach (var provider in _providers)
            {
                provider.Dispose();
            }
        }
    }
}
