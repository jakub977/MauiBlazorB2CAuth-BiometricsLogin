using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;

namespace Principal.Telemedicine.Shared.Infrastructure;
/// <summary>
/// Třída řeší registraci v rámci IServiceCollection
/// </summary>
public static class DiRegistration
{
    /// <summary>
    /// Přidává a registruje konfiguraci aplikace a přidává handler pro Http klienta.
    /// </summary>    
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    
    /// <returns></returns>
    public static IServiceCollection AddTmInfrastructure(this IServiceCollection services, IConfiguration configuration) 
    {
        services.AddHttpContextAccessor();
        services.Configure<TmAppConfiguration>(configuration.GetSection(typeof(TmAppConfiguration).Name));
       
        services.AddScoped<CustomHeaderHandler>();
        services.ConfigureAll<HttpClientFactoryOptions>(options =>
        {
            options.HttpMessageHandlerBuilderActions.Add(builder =>
            {
                builder.AdditionalHandlers.Add(builder.Services.GetRequiredService<CustomHeaderHandler>());
            });
        });
        services.AddHttpClient("TmHttpClient")
              .AddHttpMessageHandler<CustomHeaderHandler>().ConfigurePrimaryHttpMessageHandler<CustomHeaderHandler>();
      


        return services;
    }
}
