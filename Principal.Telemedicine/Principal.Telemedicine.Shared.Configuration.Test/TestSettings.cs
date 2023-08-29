namespace Principal.Telemedicine.Shared.Configuration.Test;
public class TestSettings
{
    public string PublicProperty { get; set; }

    [SecretValue]
    public string SecretProperty { get; set; }
}
