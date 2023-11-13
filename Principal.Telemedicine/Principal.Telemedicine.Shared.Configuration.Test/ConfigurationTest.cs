using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace Principal.Telemedicine.Shared.Configuration.Test;
public class SecretConfigurationProviderTests
{


    public SecretConfigurationProviderTests()
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "local");
    }
    [Fact(DisplayName ="Test pro načtení hodnot z secrets pouze")]
    public void ShouldLoadSecretValueFromSecretJson()
    {
        // Arrange
        var secretFilePath = "secured/secrets.json";
        
        // Vytvoření IConfiguration souboru s testovacími hodnotami
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json").Build();
        var mock = new Mock<ILogger<SecretConfigurationProviderTests>>();
        ILogger<SecretConfigurationProviderTests> logger = mock.Object;

        // Act
        var secretConfigProvider = new SecretConfigurationProvider<TestSettings>(configuration, secretFilePath,logger, true);
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
        var mock = new Mock<ILogger<SecretConfigurationProviderTests>>();
        ILogger<SecretConfigurationProviderTests> logger = mock.Object;
        
        // Act
        var hostBuilder = new HostBuilder().UseEnvironment("local")
        .UseSecretConfiguration<TestSettings>(configuration, logger, secretFilePath).UseSecretConfiguration<TestSettings2>(configuration, logger, secretFilePath);


        var host = new DependencyResolverHelper(hostBuilder.Build());
       
        var options = host.GetService<IOptions<TestSettings>>();
        var options2 = host.GetService<IOptions<TestSettings2>>();


        // Assert

        Assert.NotNull(options);
        Assert.NotNull(options.Value);

        Assert.NotNull(options2);
        Assert.NotNull(options2?.Value);

        Assert.Equal("TestString2", options2?.Value.StringTest);
        Assert.Equal(2, options2?.Value.NumberTest);

        Assert.Equal("PublicValueFromAppsettingsJson", options.Value.PublicProperty);
        Assert.Equal("SecretTestValue", options.Value.SecretProperty);

    }

    [Fact(DisplayName = "Test pro nastavení SecretConfigurationProvider v IServiceCollection")]
    public void ShouldSetSecretConfigurationProviderInIServiceCollection()
    {
        // Arrange
        var secretFilePath = "secured/secrets.json";
        var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json")
           .Build();
       // var mock = new Mock<ILogger<SecretConfigurationProviderTests>>();
       // ILogger<SecretConfigurationProviderTests> logger = mock.Object;

        // Act
        var hostBuilder = new HostBuilder().UseEnvironment("local")
           .ConfigureServices((hostContext, services) =>
           {
               services.AddSecretConfiguration<TestSettings>(configuration, secretFilePath);
               services.AddSecretConfiguration<TestSettings2>(configuration, secretFilePath);
           });


        var host = new DependencyResolverHelper(hostBuilder.Build());

        var options = host.GetService<IOptions<TestSettings>>();
        var options2 = host.GetService<IOptions<TestSettings2>>();



        // Assert

        Assert.NotNull(options);
        Assert.NotNull(options.Value);
        Assert.Equal("PublicValueFromAppsettingsJson", options.Value.PublicProperty);     
        Assert.Equal("SecretTestValue", options.Value.SecretProperty);
        Assert.Equal("TestString2", options2.Value.StringTest);
        Assert.Equal(2, options2.Value.NumberTest);
    }





    [Fact(DisplayName = "Test pro načtení kombinovaných hodnot - jak z Secrets, tak z appsettings.json")]
    public void ShouldLoadNormalValueFromAppsettingsJson()
    {
        // Arrange
        var secretFilePath = "secured/secrets.json";

        // Vytvoření IConfiguration souboru s testovacími hodnotami
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json").Build();
        var mock = new Mock<ILogger<SecretConfigurationProviderTests>>();
        ILogger<SecretConfigurationProviderTests> logger = mock.Object;
        // Act
        var secretConfigProvider = new SecretConfigurationProvider<TestSettings>(configuration, secretFilePath,logger, true);
        secretConfigProvider.Load();

    
        secretConfigProvider.TryGet("TestSettings:PublicProperty", out string pubValue);
        secretConfigProvider.TryGet("TestSettings:SecretProperty", out string secValue);

        // Assert
        Assert.Equal("PublicValueFromAppsettingsJson", pubValue);
        Assert.Equal("SecretTestValue", secValue);
    }
}
