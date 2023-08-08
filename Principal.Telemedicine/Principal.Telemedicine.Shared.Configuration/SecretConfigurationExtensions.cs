using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Principal.Telemedicine.Shared.Configuration;


/// <summary>
/// Třída rozšiřuje IHostBuildera pro přidání konfigurace s použitím SecretValue atributu. 
/// </summary>
public static class SecretConfigurationExtensions
{

    /// <summary>
    /// Metoda přidává nastavení konfigurace pro aplikaci s možností načítání secret value - z Secret.json, nebo Key Vault z Azure.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="hostBuilder">Aktuální host builder</param>
    /// <param name="configuration">Aktuální konfigurace. Typicky appsettings.json - instance IConfiguration. </param>
    /// <param name="secretsPath">Relativní cesta k json s secrets. V případě použití z Key Vault může být prázdné.</param>
    /// <returns></returns>
    public static IHostBuilder UseSecretConfiguration<T>(this IHostBuilder hostBuilder, IConfiguration configuration, ILogger<SecretConfigurationExtensions> logger,  string secretsPath) where T : class
    {

        return hostBuilder.ConfigureAppConfiguration((context, config) =>
        {

            bool isLocalHosted = context.HostingEnvironment.IsLocalHosted();
            var secretConfigSource = new SecretConfigurationSource<T>(configuration, secretsPath,logger, isLocalHosted);
            config.Add(secretConfigSource);    
           
        }).ConfigureServices((hostContext, services) =>
        {
            services.Configure<T>(hostContext.Configuration.GetSection(typeof(T).Name));
        });

    }
}
