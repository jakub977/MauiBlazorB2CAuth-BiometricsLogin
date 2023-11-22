namespace Principal.Telemedicine.Shared.Configuration;

/// <summary>
/// Konfigurační třída pro odeslání emailu z AD Principal engineering jménem uživatele PETelemedicina@principal.cz
/// </summary>
public class MailSettings
{
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

    /// <summary>
    /// Identifikátor tenantu. 
    /// </summary>
    [SecretValue]
    public string? TenantId { get; set; }

    /// <summary>
    /// Identifikátor objektu respektive uživatele. 
    /// </summary>
    [SecretValue]
    public string? ObjectId { get; set; }
}
