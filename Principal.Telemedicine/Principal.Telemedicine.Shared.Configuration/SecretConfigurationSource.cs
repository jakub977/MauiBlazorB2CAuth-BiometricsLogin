using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Principal.Telemedicine.Shared.Configuration;
public class SecretConfigurationSource<T> : IConfigurationSource where T : class
{
    private readonly string secretFilePath;

    public SecretConfigurationSource(string secretFilePath)
    {
        this.secretFilePath = secretFilePath;
    }

    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        return new SecretConfigurationProvider<T>(builder.Build(), secretFilePath);
    }
}
