

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace Principal.Telemedicine.Shared.Logging.Test.Helpers;
public class DependencyResolverHelper
{
    private readonly IHost host;

    public DependencyResolverHelper(IHost host)
    {
        this.host = host;
    }

    public T GetService<T>() where T : notnull
    {
        IServiceScope serviceScope = this.host.Services.CreateScope();
        IServiceProvider services = serviceScope.ServiceProvider;
        try
        {
            T scopedService = services.GetRequiredService<T>();
            return scopedService;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
