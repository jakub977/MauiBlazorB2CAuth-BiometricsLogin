
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Principal.Telemedicine.Shared.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ME = Microsoft.Extensions.Caching.Distributed;
using Principal.Telemedicine.Shared.Interfaces;

namespace Principal.Telemedicine.Shared.Cache;
public static class DiRegistrationRedis
{
    public static IServiceCollection AddDistributedCache(this IServiceCollection services,IConfiguration configuration, bool isLocal)
    {
        

    
        if (!isLocal)
        {
            var redisConfig = (DistributedRedisCacheOptions) configuration.GetSection(typeof(DistributedRedisCacheOptions).Name);
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = $"{redisConfig.Host}:{redisConfig.Port},password={redisConfig.Password},ssl=True,abortConnect=False";
            });
            services.Configure<DistributedCacheOptions>(configuration.GetSection(typeof(DistributedRedisCacheOptions).Name));
            services.AddSingleton<IDistributedCache, DistributedRedisCache>();
        }
        else
        {
            services.AddMemoryCache();
            services.AddSingleton<IDistributedCache, DistributedMemoryCache>();
        }

        return services;
    }
}
