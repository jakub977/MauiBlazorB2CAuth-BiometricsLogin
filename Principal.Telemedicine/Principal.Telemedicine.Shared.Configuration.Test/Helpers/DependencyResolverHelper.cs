
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace Principal.Telemedicine.Shared.Configuration.Test;
/// <summary>
/// Helper pro získávání registrovaných služeb DI
/// </summary>
public class DependencyResolverHelper : IDisposable
{
    private readonly IServiceScope serviceScope;

    public DependencyResolverHelper(IHost host)
    {
        this.serviceScope = host.Services.CreateScope();
    }

    public T GetService<T>() where T : notnull
    {
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

    public void Dispose()
    {
        serviceScope.Dispose();
    }
}
