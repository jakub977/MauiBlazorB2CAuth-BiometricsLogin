namespace Principal.Telemedicine.Shared.Configuration;

public class FcmSettings
{
    [SecretValue]
    public string JsonServiceKey { get; set; }

    [SecretValue]
    public string ApplicationIdentifier { get; set; }

    [SecretValue]
    public string ServiceAccountId { get; set; }

    [SecretValue]
    public string Scope { get; set; }
}

