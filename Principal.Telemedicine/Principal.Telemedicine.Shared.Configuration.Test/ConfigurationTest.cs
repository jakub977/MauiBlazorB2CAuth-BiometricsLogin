using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
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
            .AddJsonFile("appsettings.json").Build();
           

        // Act
        var secretConfigProvider = new SecretConfigurationProvider<TestSettings>(configuration, secretFilePath, true);
        secretConfigProvider.Load();

     
        secretConfigProvider.TryGet("TestSettings:SecretProperty", out string loadedValue);
        // Assert
        Assert.Equal("SecretTestValue", loadedValue);
    }

    [Fact(DisplayName = "Test pro nastavení SecretConfigurationProvider v IHostBuilderu")]
    public void ShouldSetSecretConfigurationProviderInHostBuilder()
    {
        // Arrange
        var secretFilePath = "secured/secrets.json";
        var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json")
           .Build();

        // Act
        var hostBuilder = new HostBuilder()
        .UseSecretConfiguration<TestSettings>(configuration, secretFilePath);
        
        var host = new DependencyResolverHelper(hostBuilder.Build());
       
        var options = host.GetService<IOptions<TestSettings>>();



        // Assert

        Assert.NotNull(options);
        Assert.NotNull(options.Value);
        Assert.Equal("PublicValueFromAppsettingsJson", options.Value.PublicProperty);
        Assert.Equal("SecretTestValue", options.Value.SecretProperty);

    }







    [Fact(DisplayName = "Test pro načtení kombinovaných hodnot - jak z Secrets, tak z appsettings.json")]
    public void ShouldLoadNormalValueFromAppsettingsJson()
    {
        // Arrange
        var secretFilePath = "secured/secrets.json";

        // Vytvoření IConfiguration souboru s testovacími hodnotami
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json").Build();

        // Act
        var secretConfigProvider = new SecretConfigurationProvider<TestSettings>(configuration, secretFilePath, true);
        secretConfigProvider.Load();

    
        secretConfigProvider.TryGet("TestSettings:PublicProperty", out string pubValue);
        secretConfigProvider.TryGet("TestSettings:SecretProperty", out string secValue);

        // Assert
        Assert.Equal("PublicValueFromAppsettingsJson", pubValue);
        Assert.Equal("SecretTestValue", secValue);
    }
}
