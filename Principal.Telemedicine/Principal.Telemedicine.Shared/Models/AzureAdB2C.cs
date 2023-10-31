using Principal.Telemedicine.Shared.Configuration;

namespace Principal.Telemedicine.B2CApi;

public class AzureAdB2C
{
    /// <summary>
    /// Veřejný identifikátor tenantu. 
    /// </summary>
    public string? TenantId { get; set; }

    /// <summary>
    /// Veřejný identifikátor klienta. 
    /// </summary>
    public string? ClientId { get; set; }

    /// <summary>
    /// Veřejný secret klienta. 
    /// </summary>
    public string? ClientSecret { get; set; }

    /// <summary>
    /// Veřejný identifikátor rozšíření klienta. 
    /// </summary>
    public string? ExtensionClientId { get; set; }

    /// <summary>
    /// Veřejná aplikační doména. 
    /// </summary>
    public string? ApplicationDomain { get; set; }

    /// <summary>
    /// Skrytý identifikátor tenantu. 
    /// </summary>
    [SecretValue]
    public string? STenantId { get; set; }

    /// <summary>
    /// Skrytý identifikátor klienta. 
    /// </summary>
    [SecretValue]
    public string? SClientId { get; set; }

    /// <summary>
    /// Skrytý secret klienta. 
    /// </summary>
    [SecretValue]
    public string? SClientSecret { get; set; }

    /// <summary>
    /// Skrytý identifikátor rozšíření klienta  
    /// </summary>
    [SecretValue]
    public string? SExtensionClientId { get; set; }

    /// <summary>
    /// Skrytá aplikační doména. 
    /// </summary>
    [SecretValue]
    public string? SApplicationDomain { get; set; }
}
