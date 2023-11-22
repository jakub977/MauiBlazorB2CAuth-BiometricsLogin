namespace Principal.Telemedicine.Shared.Configuration;

/// <summary>
/// Konfigurační třída pro komunikaci s Firebase Clound Messaging
/// </summary>
public class FcmSettings
{
    /// <summary>
    /// Konfigurační JSON služby FCM
    /// </summary>
    [SecretValue]
    public string JsonServiceKey { get; set; }

    /// <summary>
    /// Identifikátor aplikace zaregistrované na FCM
    /// </summary>
    [SecretValue]
    public string ApplicationIdentifier { get; set; }

    /// <summary>
    /// Servisní účet
    /// </summary>
    [SecretValue]
    public string ServiceAccountId { get; set; }

    /// <summary>
    /// Oblast služeb FCM
    /// </summary>
    [SecretValue]
    public string Scope { get; set; }
}

