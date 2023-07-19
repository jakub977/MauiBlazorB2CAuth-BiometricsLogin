using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Principal.Telemedicine.DataConnectors.Models;
using Principal.Telemedicine.Shared.Logging;
using System.Collections.Generic;
using Microsoft.Extensions.Logging.AzureAppServices;

public static class DiRegistration
{
    /// <summary>
    /// Přidání logování do aplikace
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration">instance IConfiguration</param>
    /// <returns></returns>
    public static IServiceCollection AddLogging(this IServiceCollection services, IConfiguration configuration)
    {
    
        // Vytvoříme instance logovacích providerů pro TelemedicineDbLogger
        var telemedicineLoggerProvider = new TelemedicineLoggerProvider(services.BuildServiceProvider().GetService<DbContextGeneral>());

        // Přidáme do DI kontejneru vlastní CompositeLoggerProvider
        services.AddSingleton<IEnumerable<ILoggerProvider>>(new List<ILoggerProvider>
        {
            telemedicineLoggerProvider
        });

       
        services.AddSingleton<ILoggerProvider, CompositeLoggerProvider>();
        services.Configure<AzureFileLoggerOptions>(configuration.GetSection("AzureFileLogging"));
        services.AddLogging(builder =>
        {        
            builder.AddProvider(services.BuildServiceProvider().GetService<ILoggerProvider>()); 
        });

        return services;
    }
}
