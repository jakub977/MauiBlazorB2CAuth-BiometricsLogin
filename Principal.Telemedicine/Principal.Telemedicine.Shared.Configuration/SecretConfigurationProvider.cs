using Microsoft.Extensions.Configuration;
using System;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Azure.Security.KeyVault.Secrets;
using Azure.Identity;

namespace Principal.Telemedicine.Shared.Configuration;
public class SecretConfigurationProvider<T> : ConfigurationProvider where T: class
{
    private readonly IConfigurationRoot _configuration;
    private readonly string _secretPath;
    private readonly bool _isLocal;
    

    public SecretConfigurationProvider(IConfigurationRoot configuration, string secretPath, bool isLocal = false )
    {
        this._configuration = configuration;
        this._secretPath = secretPath;
        this._isLocal = isLocal;
    }

    public override void Load()
    {

        IConfigurationRoot secretConfig = null;
        try
        {
            string keyVaultEndpoint = string.Empty;
            SecretClient? secretClient = null;
            if (_isLocal)
            {
                secretConfig = new ConfigurationBuilder().AddJsonFile(_secretPath).Build();

            }
            else
            {
                // Načíst hodnoty z Azure Key Vault
                 keyVaultEndpoint = _configuration[ConfigConstants.CONFIG_KEY_VAULT_URL];
                 secretClient = new SecretClient(new Uri(keyVaultEndpoint), new DefaultAzureCredential());
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
                        var secretName = $"{typeof(T).Name}:{property.Name}";
                        var secret = secretClient.GetSecret(secretName);
                        Data[property.Name] = secret.Value.Value;
                    }
                    else
                    {
                        // Načtěte hodnotu z "secrets.json" (lokální režim)
                        var secretValue = secretConfig[$"{typeof(T).Name}:{property.Name}"];
                        Data[property.Name] = secretValue;
                    }
                }
                else
                {
                    // Načtěte hodnotu z běžné konfigurace (appsettings.json)
                    var normalValue = _configuration[$"{typeof(T).Name}:{property.Name}"];
                    Data[property.Name] = normalValue;
                }
            }
        }
        catch(Exception ex)
        {
            //TODO Loging
            throw;
        }
    }
}
