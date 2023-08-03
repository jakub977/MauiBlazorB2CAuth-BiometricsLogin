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
    private readonly bool _isLocal;

    public SecretConfigurationSource(string secretFilePath, bool isLocal = false)
    {
        this.secretFilePath = secretFilePath;
        _isLocal = isLocal;
    }

    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        return new SecretConfigurationProvider<T>(builder.Build(), secretFilePath, _isLocal);
    }
}
