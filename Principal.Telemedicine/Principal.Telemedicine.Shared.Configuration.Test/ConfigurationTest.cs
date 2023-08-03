using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Principal.Telemedicine.Shared.Configuration;
using Xunit;
namespace Principal.Telemedicine.Shared.Configuration.Test;
public class SecretConfigurationProviderTests
{


    public SecretConfigurationProviderTests()
    {

    }
    [Fact(DisplayName ="Test pro načtení hodnot z secrets pouze")]
    public void ShouldLoadSecretValueFromSecretJson()
    {
        // Arrange
        var secretFilePath = "secured/secrets.json";

        // Vytvoření IConfiguration souboru s testovacími hodnotami
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        // Act
        var secretConfigProvider = new SecretConfigurationProvider<TestSettings>(configuration, secretFilePath, true);
        secretConfigProvider.Load();

     
        secretConfigProvider.TryGet("SecretProperty", out string loadedValue);
        // Assert
        Assert.Equal("SecretValueFromSecretJson", loadedValue);
    }

    [Fact(DisplayName = "Test pro nastavení SecretConfigurationProvider v IHostBuilderu")]
    public void ShouldSetSecretConfigurationProviderInHostBuilder()
    {
        // Arrange
        var secretFilePath = "secured/secrets.json";
        var isLocal = true;

        // Act
        var hostBuilder = new HostBuilder()
            .UseSecretConfiguration<TestSettings>(secretFilePath) // Volání extension metody UseSecretConfiguration
            .ConfigureServices((context, services) =>
            {
                // Registrace služby s konfigurací
                services.Configure<TestSettings>(context.Configuration);
            });

        var host = hostBuilder.Build();
        var configurationProvider = host.Services.GetRequiredService<IConfigurationProvider>() as SecretConfigurationProvider<TestSettings>;
        var options = host.Services.GetRequiredService<IOptions<TestSettings>>();
        // Assert

        Assert.NotNull(configurationProvider);
        Assert.NotNull(options.Value);
        Assert.Equal("PublicValueFromAppsettingsJson", options.Value.PublicProperty);
        Assert.Equal("SecretValueFromSecretJson", options.Value.SecretProperty);

    }







    [Fact(DisplayName = "Test pro načtení kombinovaných hodnot - jak z Secrets, tak z appsettings.json")]
    public void ShouldLoadNormalValueFromAppsettingsJson()
    {
        // Arrange
        var secretFilePath = "secured/secrets.json";

        // Vytvoření IConfiguration souboru s testovacími hodnotami
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        // Act
        var secretConfigProvider = new SecretConfigurationProvider<TestSettings>(configuration, secretFilePath);
        secretConfigProvider.Load();

    
        secretConfigProvider.TryGet("PublicProperty", out string pubValue);
        secretConfigProvider.TryGet("SecretProperty", out string secValue);

        // Assert
        Assert.Equal("PublicValueFromAppsettingsJson", pubValue);
        Assert.Equal("SecretValueFromSecretJson", secValue);
    }
}
