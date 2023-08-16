using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;

namespace Principal.Telemedicine.Shared.Configuration;

/// <summary>
/// Třída řeší registraci v rámci IServiceCollection
/// </summary>
public static class DiRegistration
{
    /// <summary>
    /// Přidává a registruje konfiguraci
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="secretPath">Cesta k json se secrets. V produkci se čte z Kez Vault</param>
    /// <returns></returns>
    public static IServiceCollection AddSecretConfiguration<T>(this IServiceCollection services, IConfiguration configuration, string secretPath) where T : class
    {

        services.AddSingleton<IConfiguration>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<T>>();
            var hostEnironment = sp.GetRequiredService<IHostEnvironment>();
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
            .Add(new SecretConfigurationSource<T>(configuration, secretPath, logger, hostEnironment.IsLocalHosted()));
            return configurationBuilder.Build();
        });

        services.AddOptions<T>().Configure<IConfiguration>((options, configuration) =>
        {
            var section = configuration.GetSection(typeof(T).Name);
            section.Bind(options);
        });


        return services;
    }

}
