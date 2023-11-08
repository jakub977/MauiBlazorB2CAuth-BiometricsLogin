using Principal.Telemedicine.Shared.Configuration;

namespace Principal.Telemedicine.B2CApi;

/// <summary>
/// Třída definující privátní a veřejné credentials nutné k autorizaci požadavku při volání API Connectoru.
/// </summary>
public class AuthorizationSettings
{
    /// <summary>
    /// Email nutný k autorizace vůči AD.
    /// </summary>
    [SecretValue]
    public string? Email { get; set; }

    /// <summary>
    /// Heslo nutné k autorizaci vůči AD.
    /// </summary>
    [SecretValue]
    public string? Password { get; set; }

}
