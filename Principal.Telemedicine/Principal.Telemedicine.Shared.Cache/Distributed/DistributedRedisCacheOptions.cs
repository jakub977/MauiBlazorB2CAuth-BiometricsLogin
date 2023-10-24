using Principal.Telemedicine.Shared.Configuration;

namespace Principal.Telemedicine.Shared.Cache;
/// <inheritdoc />
public class DistributedRedisCacheOptions : DistributedCacheOptions
{
    /// <summary>
    /// Port na kterém poslouchá distributed cache.
    /// </summary>        
    public int Port { get; set; }

    /// <summary>
    /// Host Distributed cache serveru.
    /// </summary>
    public string Host { get; set; }

    /// <summary>
    /// Heslo pro připojení k DC Reedis.
    /// </summary>
    [SecretValue]
    public string Password { get; set; }


}
