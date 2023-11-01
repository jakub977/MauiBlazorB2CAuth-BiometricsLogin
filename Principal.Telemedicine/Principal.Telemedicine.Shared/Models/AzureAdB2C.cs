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
    public string? B2cExtensionAppClientId { get; set; }

    /// <summary>
    /// Veřejná aplikační doména. 
    /// </summary>
    public string? B2CApplicationDomain { get; set; }

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
    public string? SB2cExtensionAppClientId { get; set; }

    /// <summary>
    /// Skrytá aplikační doména. 
    /// </summary>
    [SecretValue]
    public string? SB2CApplicationDomain { get; set; }
}
