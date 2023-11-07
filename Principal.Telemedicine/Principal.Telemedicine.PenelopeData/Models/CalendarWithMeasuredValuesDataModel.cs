using System.Text.Json.Serialization;
using Principal.Telemedicine.Shared.Models;

namespace Principal.Telemedicine.PenelopeData.Models;

/// <summary>
/// Data model of patients measured values in calendar - specific for Penelope.
/// </summary>
public class CalendarWithMeasuredValuesDataModel : ACalendarWithMeasuredValuesDataModel
{   /// <summary>
    /// Patients week of pregnancy
    /// </summary>
    [JsonPropertyName("WeekOfPregnancy")]
    public string? WeekOfPregnancy { get; set; }
}
