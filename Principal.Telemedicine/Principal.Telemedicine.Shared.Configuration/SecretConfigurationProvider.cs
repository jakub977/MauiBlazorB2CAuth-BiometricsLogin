using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Reflection;


namespace Principal.Telemedicine.Shared.Configuration;
/// <summary>
/// Provider pro IConfigurationSource. Umožňuje načítat konfiguraci z aplikačního nastavení, z secret.json a Key Vault
/// </summary>
/// <typeparam name="T"></typeparam>
public class SecretConfigurationProvider<T> : ConfigurationProvider where T: class
{
    private readonly IConfiguration _configuration;
    private readonly string _secretPath;
    private readonly bool _isLocal;
    private readonly ILogger _logger;

    /// <summary>
    /// Konstruktor provideru. 
    /// </summary>
    /// <param name="configuration">Aktuální konfigurace. Musí především obsahovat connection k KeyVault (KEY_VAULT_URL), pokud se nejedná o lokální instanci. </param>
    /// <param name="secretPath">Cesta k secrets.json.</param>
    /// <param name="logger">Instance ILoggeru.</param>
    /// <param name="isLocal">Příznak, jestli se jedná o lokální prostředí - pokud true, načítají se secrets z secret.json, jinak z Key Vault.</param>
    public SecretConfigurationProvider(IConfiguration configuration, string secretPath, ILogger logger, bool isLocal = false )
    {
        this._configuration = configuration;
        this._secretPath = secretPath;
        this._isLocal = isLocal;
        this._logger = logger;
    }

    /// <summary>
    /// Načítání katalogu konfigurace na základě parametrů. 
    /// </summary>
    public override void Load()
    {

        IConfigurationRoot secretConfig = null;
        try
        {
            string keyVaultEndpoint = string.Empty;
            SecretClient? secretClient = null;
            if (_isLocal)
            {
               if(File.Exists(_secretPath)) secretConfig = new ConfigurationBuilder().AddJsonFile(_secretPath).Build();

            }
            else
            {
                try
                {
                    // Načíst hodnoty z Azure Key Vault
                    keyVaultEndpoint = _configuration[ConfigConstants.CONFIG_KEY_VAULT_URL];
                    secretClient = new SecretClient(new Uri(keyVaultEndpoint), new DefaultAzureCredential());
                }catch(Exception ex)
                {
                    _logger.LogError(ex, "Chyba vytvoření Key Vault klienta");
                    throw;
                }
            }
            var formatSettingsProperties = typeof(T).GetProperties();
            foreach (var property in formatSettingsProperties)
            {
                var secretValueAttribute = property.GetCustomAttribute<SecretValueAttribute>();
                if (secretValueAttribute != null)
                {
                    // Načtěte hodnotu z Azure Key Vault, pokud není nastaven _isLocal
                    if (!_isLocal)
                    {
                        try
                        {
                            var secretName = $"{typeof(T).Name}:{property.Name}";
                            var secret = secretClient.GetSecret(secretName?.Replace(":", String.Empty))?.Value;
                            Data[$"{typeof(T).Name}:{property.Name}"] = secret.Value;
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Chyba načtení Key Vault hodnoty");
                            throw;
                        }
                     }
                    else
                    {
                        // Načtěte hodnotu z "secrets.json" (lokální režim)
                        var secretValue = secretConfig[$"{typeof(T).Name}:{property.Name}"];
                        Data[$"{typeof(T).Name}:{property.Name}"] = secretValue;
                    }
                }
                else
                {
                    // Načtěte hodnotu z běžné konfigurace (appsettings.json)
                    var normalValue = _configuration[$"{typeof(T).Name}:{property.Name}"];
                    Data[$"{typeof(T).Name}:{property.Name}"] = normalValue;
                }
            }
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Error in registering configuration provider");
            throw;
        }
    }
}
