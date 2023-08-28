using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Principal.Telemedicine.Shared.Infrastructure;
/// <summary>
/// Třída řeší registraci v rámci IServiceCollection
/// </summary>
public static class DiRegistration
{
    /// <summary>
    /// Přidává a registruje konfiguraci
    /// </summary>    
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    
    /// <returns></returns>
    public static IServiceCollection AddTmInfrastructure(this IServiceCollection services, IConfiguration configuration) 
    {
        services.AddHttpContextAccessor();

        services.ConfigureOptions<TmAppConfigurationSetup>();        
        services.AddTransient<CustomHeaderHandler>();
        services.AddHttpClient<TmHttpClient>()
              .AddHttpMessageHandler<CustomHeaderHandler>();
        
        return services;
    }
}
