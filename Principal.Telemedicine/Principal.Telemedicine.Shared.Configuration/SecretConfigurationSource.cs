using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Principal.Telemedicine.Shared.Configuration;
/// <summary>
/// Zdroj konfigurace pro použití s custom config providerem. 
/// </summary>
/// <typeparam name="T"></typeparam>
public class SecretConfigurationSource<T> : IConfigurationSource where T : class
{
    private readonly string secretFilePath;
    private readonly bool _isLocal;
    private readonly ILogger _logger;
    private readonly IConfiguration _config;

    public SecretConfigurationSource(IConfiguration config, string secretFilePath, ILogger logger, bool isLocal = false)
    {
        this.secretFilePath = secretFilePath;
        _isLocal = isLocal;
        _config = config;
        _logger = logger;
    }

    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        return new SecretConfigurationProvider<T>(_config, secretFilePath, _logger, _isLocal);
    }
}
