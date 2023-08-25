using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Principal.Telemedicine.Shared.Intrastructure;
using Moq;
using Principal.Telemedicine.Shared.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Principal.Telemedicine.Shared.Intrastructure.Test;

public class TmAppConfigureTest
{
    private DependencyResolverHelper serviceProvider;

    public TmAppConfigureTest()
    {
        // Arrange
        var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json")
            .Build();

        // Registr logging
        var hostBuilder = new HostBuilder()
            .ConfigureServices((context, services) =>
            {
                
                services.AddTmInfrastructure(configuration);
            });

         serviceProvider = new DependencyResolverHelper(hostBuilder.Build());
    }

    [Fact(DisplayName = "Test konfigurace prostředí aplikace")]
    public void TestKonfigurace()
    {
        var iOption = serviceProvider.GetService<IOptions<TmAppConfiguration>>();
        Assert.NotNull(iOption);
        Assert.Equal("-", iOption.Value.IdentificationDelimeter);
        Assert.Equal("testApi", iOption.Value.IdentificationId, true );
    }
}