using System.Text.Json.Serialization;
namespace Principal.Telemedicine.Shared.Models;

public partial class ACalendarWithMeasuredValuesDataModel //toto udělat jako partial class, vytvořit projekt Principal.Telemedicine.PenelopeData / a  atyk VandaData (tam dávat data modely) - udělat referenci z těchto projektů do data connectors
{
    public string? DataType { get; set; }
    public string? MeasuredValue { get; set; }
    public int? DataTypeId { get; set; }
    public string? CreationType { get; set; }

    [JsonIgnore]
    public string? Day { get; set; }

    [JsonIgnore]
    public DateTime CalendarDateUtc { get; set; }
    public string DayDate =>
        string.Format("{0} {1}", this.Day, this.CalendarDateUtc);
    public TimeSpan? MeasuredTime { get; set; }
}
