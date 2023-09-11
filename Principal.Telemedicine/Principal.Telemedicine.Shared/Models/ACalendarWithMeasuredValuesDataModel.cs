using System.Text.Json.Serialization;
namespace Principal.Telemedicine.Shared.Models;

public abstract class ACalendarWithMeasuredValuesDataModel
{
    public string? DataType { get; set; }
    public string MeasuredValue { get; set; }
    public int? DataTypeId { get; set; }

    [JsonIgnore]
    public string Day { get; set; }

    [JsonIgnore]
    public DateTime CalendarDateUtc { get; set; }
    public string DayDate =>
        string.Format("{0} {1}", this.Day, this.CalendarDateUtc);
    public TimeSpan? MeasuredTime { get; set; }
}
