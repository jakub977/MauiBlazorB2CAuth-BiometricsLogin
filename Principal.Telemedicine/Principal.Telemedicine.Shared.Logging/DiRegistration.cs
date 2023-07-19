using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Principal.Telemedicine.DataConnectors.Models;
using Principal.Telemedicine.Shared.Logging;
using System.Collections.Generic;
using Microsoft.Extensions.Logging.AzureAppServices;

public static class DiRegistration
{
    public static IServiceCollection AddLogging(this IServiceCollection services, IConfiguration configuration)
    {
    
    

        // Vytvoříme instance logovacích poskytovatelů pro TelemedicineDbLogger a Azure App Log
        var telemedicineLoggerProvider = new TelemedicineLoggerProvider(services.BuildServiceProvider().GetService<DbContextGeneral>());

        // Přidáme do DI kontejneru vlastní CompositeLoggerProvider s logovacími poskytovateli
        services.AddSingleton<IEnumerable<ILoggerProvider>>(new List<ILoggerProvider>
        {
            telemedicineLoggerProvider
        });

       
        services.AddSingleton<ILoggerProvider, CompositeLoggerProvider>();

        services.AddLogging(builder =>
        {
            builder.AddConfiguration(configuration.GetSection("Logging"));        
            builder.AddAzureWebAppDiagnostics();//logování do Azure AppLog
            builder.AddProvider(services.BuildServiceProvider().GetService<ILoggerProvider>()); 
        });

        return services;
    }
}
