using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Principal.Telemedicine.Shared.Infrastructure;
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
