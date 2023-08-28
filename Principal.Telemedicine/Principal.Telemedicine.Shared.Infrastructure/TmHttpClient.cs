using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Principal.Telemedicine.Shared.Infrastructure;

/// <summary>
/// TM HttpClient. Add tracing header before call. Registering
/// services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
//  services.AddTransient<CustomHeaderHandler>();
//  services.AddHttpClient<CustomHeaderHttpClient>()
//        .AddHttpMessageHandler<CustomHeaderHandler>();
/// </summary>
public class TmHttpClient : HttpClient
{
    public TmHttpClient(CustomHeaderHandler customHeaderHandler)
        : base(customHeaderHandler)
    {
    }
}
