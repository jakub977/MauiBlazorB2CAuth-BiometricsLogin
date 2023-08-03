using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Principal.Telemedicine.Shared.Configuration;
public class SecretConfigurationProvider<T> : ConfigurationProvider where T: class
{
    private readonly IConfigurationRoot configuration;
    private readonly string secretFilePath;

    public SecretConfigurationProvider(IConfigurationRoot configuration, string secretFilePath)
    {
        this.configuration = configuration;
        this.secretFilePath = secretFilePath;
    }

    public override void Load()
    {
        // Načtěte hodnoty ze "secrets.json"
        var secretConfig = new ConfigurationBuilder()
            .AddJsonFile(secretFilePath)
            .Build();

        // Prochází se všechny vlastnosti, zda mají atribut [SecretValue],
        // a načtěte hodnoty z secrets.json nebo z běžné konfigurace podle toho, zda mají atribut nebo ne.
        var formatSettingsProperties = typeof(T).GetProperties();
        foreach (var property in formatSettingsProperties)
        {
            var secretValueAttribute = property.GetCustomAttribute<SecretValueAttribute>();
            if (secretValueAttribute != null)
            {
                // Načtěte hodnotu z "secrets.json"
                var secretValue = secretConfig[$"{typeof(T).Name}:" + property.Name];
                Data[property.Name] = secretValue;
            }
            else
            {
                // Načtěte hodnotu z běžné konfigurace (appsettings.json)
                var normalValue = configuration[$"{typeof(T).Name}:" + property.Name];
                Data[property.Name] = normalValue;
            }
        }
    }
}
