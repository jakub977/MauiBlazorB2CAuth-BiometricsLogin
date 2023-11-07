namespace Principal.Telemedicine.Shared.Configuration;

/// <summary>
/// Konfigurační třída pro volání AD B2C
/// </summary>
public class AzureAdB2C
{
    public string? Instance { get; set; }
    public string? Domain { get; set; }

    /// <summary>
    /// Identifikátor rozšíření klienta. 
    /// </summary>
    [SecretValue]
    public string? B2cExtensionAppClientId { get; set; }

    /// <summary>
    /// Identifikátor tenantu. 
    /// </summary>
    [SecretValue]
    public string? TenantId { get; set; }

    /// <summary>
    /// Identifikátor klienta. 
    /// </summary>
    [SecretValue]
    public string? ClientId { get; set; }

    /// <summary>
    /// Secret klienta. 
    /// </summary>
    [SecretValue]
    public string? ClientSecret { get; set; }
    public bool AllowWebApiToBeAuthorizedByACL { get; set; }
    public string? CallbackPath { get; set; }
    public string? SignedOutCallbackPath { get; set; }
    public string? SignUpSignInPolicyId { get; set; }

    /// <summary>
    /// Aplikační doména. 
    /// </summary>
    public string? B2CApplicationDomain { get; set; }

    public string? Scopes { get; set; }
}
