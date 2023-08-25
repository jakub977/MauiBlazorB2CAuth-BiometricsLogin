namespace Principal.Telemedicine.Shared.Infrastructure;
public class TmAppConfiguration
{
    const string IDENTIFICATION_DELIMETER = "-";
    public string? IdentificationId { get; set; }
    public string IdentificationDelimeter { get => IDENTIFICATION_DELIMETER; }
}
