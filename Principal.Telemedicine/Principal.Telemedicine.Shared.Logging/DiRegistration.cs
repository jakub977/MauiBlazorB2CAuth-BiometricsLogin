using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace Principal.Telemedicine.Shared.Logging;
public static class DiRegistration
{
    public static IServiceCollection AddLogging(this IServiceCollection services, IConfiguration configuration)
    {

        var connectionString = configuration.GetConnectionString("DbLoggerConnectionString");      
        services.AddSingleton<SqlConnectionLoggerFactory>(sp =>
          new SqlConnectionLoggerFactory(connectionString));
        services.AddSingleton<ILoggerProvider, TelemedicineLoggerProvider>();
        return services;
    }
}
