using Principal.Telemedicine.Shared.Models;

namespace Principal.Telemedicine.PenelopeData.Models;

/// <summary>
/// Data model of patients measured values in calendar - specific for Penelope.
/// </summary>
public class CalendarWithMeasuredValuesDataModel : ACalendarWithMeasuredValuesDataModel
{
    public string? WeekOfPregnancy { get; set; }
}
