namespace Principal.Telemedicine.Shared.Infrastructure;
/// <summary>
/// Konfigurace používané aplikace.
/// </summary>
public class TmAppConfiguration
{
    const string IDENTIFICATION_DELIMETER = "-";
    public string? IdentificationId { get; set; }
    public string IdentificationDelimeter { get => IDENTIFICATION_DELIMETER; }
}
