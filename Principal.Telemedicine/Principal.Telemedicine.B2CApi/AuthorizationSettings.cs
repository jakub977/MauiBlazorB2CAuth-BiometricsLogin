using Principal.Telemedicine.Shared.Configuration;

namespace Principal.Telemedicine.B2CApi;

/// <summary>
/// Třída definující privátní a veřejné credentials nutné k autorizaci požadavku při volání API Connectoru.
/// </summary>
public class AuthorizationSettings
{
    /// <summary>
    /// Veřejný email. 
    /// </summary>
    public string PEmail { get; set; }

    /// <summary>
    /// Skrytý email.
    /// </summary>
    [SecretValue]
    public string SEmail { get; set; }

    /// <summary>
    /// Veřejné heslo.
    /// </summary>
    public string PPassword { get; set; }

    /// <summary>
    /// Skryté heslo.
    /// </summary>
    [SecretValue]
    public string SPassword { get; set; }

}
