using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Principal.Telemedicine.Shared.Interfaces;
using System;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Principal.Telemedicine.Shared.Cache;
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
