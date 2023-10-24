using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Principal.Telemedicine.Shared.Interfaces;

namespace Principal.Telemedicine.Shared.Cache;
/// <summary>
/// Registrace do DI
/// </summary>
public static class DiRegistrationLc
{
    public static IServiceCollection AddTmMemoryCache(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MemoryCacheOptions>(configuration.GetSection(typeof(MemoryCacheOptions).Name));
        services.AddMemoryCache();
        services.Configure<MemoryCacheOptions>(configuration);
        services.AddSingleton<IMemoryCache, MemoryCache>();

        return services;
    }
}
