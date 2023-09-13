using System.ComponentModel.DataAnnotations.Schema;

namespace Principal.Telemedicine.Shared.Models;

/// <summary>
/// Data model plánovače pacienta s naměřenými hodnotami
/// </summary>
public partial class UserCalendarWithMeasuredValuesDataModel : ACalendarWithMeasuredValuesDataModel
{
    public int WeekOfYear { get; set; }
}
