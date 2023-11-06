using System.Text.Json.Serialization;
namespace Principal.Telemedicine.Shared.Models;

/// <summary>
/// Data model of patients measured values in calendar.
/// </summary>
public partial class ACalendarWithMeasuredValuesDataModel
{
    /// <summary>
    /// Type of measured value
    /// </summary>
    [JsonPropertyName("DataType")]
    public string? DataType { get; set; }

    /// <summary>
    /// Measured value
    /// </summary>
    [JsonPropertyName("MeasuredValue")]
    public string? MeasuredValue { get; set; }

    /// <summary>
    /// Identifier of data type
    /// </summary>
    [JsonPropertyName("DataTypeId")]
    public int? DataTypeId { get; set; }

    /// <summary>
    /// Type of values creation
    /// </summary>
    [JsonPropertyName("CreationType")]
    public string? CreationType { get; set; }

    /// <summary>
    /// Day in which the value is measured.
    /// </summary>
    [JsonIgnore]
    public string? Day { get; set; }

    /// <summary>
    /// Date of measured value creation, using coordinated universal time
    /// </summary>
    [JsonIgnore]
    public DateTime CalendarDateUtc { get; set; }

    /// <summary>
    /// Day and date of measured value creation, using coordinated universal time
    /// </summary>
    [JsonPropertyName("DayDate")]
    public string DayDate =>
        string.Format("{0} {1}", this.Day, this.CalendarDateUtc);

    /// <summary>
    /// Time of measured value creation
    /// </summary>
    [JsonPropertyName("MeasuredTime")]
    public TimeSpan? MeasuredTime { get; set; }
}
