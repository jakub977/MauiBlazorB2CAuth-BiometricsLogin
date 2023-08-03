using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Principal.Telemedicine.Shared.Configuration;

public static class SecretConfigurationExtensions
{
    public static IHostBuilder UseSecretConfiguration<T>(this IHostBuilder hostBuilder, string secretsPath) where T : class
    {
        return hostBuilder.ConfigureAppConfiguration((context, config) =>
        {
            var configuration = config.Build();
            bool isLocalHosted = context.HostingEnvironment.IsLocalHosted();
            var secretConfigSource = new SecretConfigurationSource<T>(secretsPath, isLocalHosted);
            config.Add(secretConfigSource);
        });
    }
}
