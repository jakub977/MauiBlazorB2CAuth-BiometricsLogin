using System.ComponentModel.DataAnnotations.Schema;

namespace Principal.Telemedicine.Shared.Models;

/// <summary>
/// Data model plánovače pacienta s naměřenými hodnotami
/// </summary>
public partial class UserCalendarWithMeasuredValuesDataModel : ACalendarWithMeasuredValuesDataModel
{
    /// <summary>
    /// A period of specific seven days of calendar year 
    /// </summary>
    public int WeekOfYear { get; set; }
}
