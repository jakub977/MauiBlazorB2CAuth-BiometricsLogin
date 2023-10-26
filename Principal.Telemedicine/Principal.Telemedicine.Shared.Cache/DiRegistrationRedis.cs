
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Principal.Telemedicine.Shared.Interfaces;

namespace Principal.Telemedicine.Shared.Cache;
/// <summary>
/// Registrace do DI
/// </summary>
public static class DiRegistrationRedis
{
    public static IServiceCollection AddTmDistributedCache(this IServiceCollection services,IConfiguration configuration, bool isLocal)
    {
        

    
        if (!isLocal)
        {
            var redisConfig = configuration.GetSection(typeof(DistributedRedisCacheOptions).Name);
            services.Configure<DistributedRedisCacheOptions>(redisConfig);

            var config = services.BuildServiceProvider().GetService<IOptions<DistributedRedisCacheOptions>>().Value;

            services.AddStackExchangeRedisCache(options =>
            {

                options.Configuration = $"{config.Host}:{config.Port},password={config.Password},ssl=True,abortConnect=False";
            });
            services.Configure<DistributedCacheOptions>(configuration.GetSection(typeof(DistributedRedisCacheOptions).Name));
            services.AddSingleton<IDistributedCache, DistributedRedisCache>();
        }
        else
        {
            services.AddMemoryCache();
            var memoryConf = configuration.GetSection(typeof(DistributedRedisCacheOptions).Name);
            services.Configure<IOptions<DistributedCacheOptions>>(memoryConf);
            services.AddSingleton<IDistributedCache, DistributedMemoryCache>();
        }

        return services;
    }
}
