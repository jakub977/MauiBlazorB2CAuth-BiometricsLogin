using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Principal.Telemedicine.Shared.Cache.Test.Helpers;
using Principal.Telemedicine.Shared.Cache;
using Principal.Telemedicine.Shared.Interfaces;
using Principal.Telemedicine.Shared.Configuration;

namespace Principal.Telemedicine.Shared.Cache.Test;

public class DiRegistrationTests
{
    private DependencyResolverHelper serviceProvider;
    private CancellationToken token = new CancellationTokenRegistration().Token;
    private const string TEST_VALUE = "Test value";
    public DiRegistrationTests()
    {
        // Arrange
        var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json")
            .Build();
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "local");
        // Registr logging
        var hostBuilder = new HostBuilder().UseEnvironment("local")
            .ConfigureServices((context, services) =>
            {
                services.AddSecretConfiguration<DistributedRedisCacheOptions>(configuration, "secrets/secrets.json");
                services.AddTmDistributedCache(configuration, true);
                services.AddTmMemoryCache(configuration);
            });

        serviceProvider = new DependencyResolverHelper(hostBuilder.UseEnvironment("production").Build());

    }
    [Fact(DisplayName ="Test of distributed cache")]
    public async Task Test1()
    {
        //Fact
        string TEST_KEY = $"TEST_DST_CACHE_{Guid.NewGuid().ToString()}";
        var service = serviceProvider.GetService<IDistributedCache>();
        await service.AddAsync(TEST_KEY, TEST_VALUE, token, new CacheEntryOptions() { SlidingExpiration = new TimeSpan(0, 0, 10) });
        Thread.Sleep(2000);
        var tst1 = await service.GetAsync<string>(TEST_KEY, token);
        Assert.Equal(TEST_VALUE, tst1);
        Thread.Sleep(10000);
        tst1 = await service.GetAsync<string>(TEST_KEY, token);
        Assert.Null(tst1);

    }


    [Fact(DisplayName = "Test of memory cache")]
    public async Task Test2()
    {
        string TEST_KEY = $"TEST_DST_CACHE_{Guid.NewGuid().ToString()}";
        var service = serviceProvider.GetService<IMemoryCache>();
        await service.AddAsync(TEST_KEY, TEST_VALUE, token, new CacheEntryOptions() { SlidingExpiration = new TimeSpan(0, 0, 10) });
        Thread.Sleep(2000);
        var tst1 = await service.GetAsync<string>(TEST_KEY, token);
        Assert.Equal(TEST_VALUE, tst1);
        Thread.Sleep(10000);
        tst1 = await service.GetAsync<string>(TEST_KEY, token);
        Assert.Null(tst1);
    }
  }