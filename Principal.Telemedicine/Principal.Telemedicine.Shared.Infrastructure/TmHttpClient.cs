namespace Principal.Telemedicine.Shared.Infrastructure;

/// <summary>
/// TM HttpClient. Přidává tracing do volání. 
/// </summary>
public class TmHttpClient : HttpClient
{
    public TmHttpClient(CustomHeaderHandler customHeaderHandler)
        : base(customHeaderHandler)
    {
    }
}
