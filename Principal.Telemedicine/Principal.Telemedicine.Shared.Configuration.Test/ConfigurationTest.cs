using Microsoft.Extensions.Configuration;

using Principal.Telemedicine.Shared.Configuration;
using Xunit;
namespace Principal.Telemedicine.Shared.Configuration.Test;
public class SecretConfigurationProviderTests
{
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
        var secretConfigProvider = new SecretConfigurationProvider<TestSettings>(configuration, secretFilePath);
        secretConfigProvider.Load();

     
        secretConfigProvider.TryGet("SecretProperty", out string loadedValue);
        // Assert
        Assert.Equal("SecretValueFromSecretJson", loadedValue);
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
