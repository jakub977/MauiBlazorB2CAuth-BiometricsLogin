namespace Principal.Telemedicine.Shared.Models;

/// <summary>
/// Data model plánovače pacienta s naměřenými hodnotami
/// </summary>
public class UserCalendarWithMeasuredValuesDataModel : ACalendarWithMeasuredValuesDataModel
{
    public int CreationType { get; set; }
    public int WeekOfYear { get; set; }
}
