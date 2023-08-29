using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Principal.Telemedicine.Shared.Infrastructure;
/// <summary>
/// Setup třída pro registraci konfigurace.
/// </summary>
public class TmAppConfigurationSetup : IConfigureOptions<TmAppConfiguration>
{
    private string SectionName = typeof(TmAppConfiguration).Name;
    private readonly IConfiguration _configuration;

    public TmAppConfigurationSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(TmAppConfiguration options)
    {
        _configuration
            .GetSection(SectionName)
            .Bind(options);
    }

}
