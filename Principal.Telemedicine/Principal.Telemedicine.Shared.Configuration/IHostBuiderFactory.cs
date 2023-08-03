using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Principal.Telemedicine.Shared.Configuration;
public interface IHostBuilderFactoryForConfiguration 
{
    IConfigurationBuilder AddConfiguration(IConfigurationBuilder builder, HostBuilderContext context);
}
