namespace Principal.Telemedicine.PenelopeData.Models;

/// <summary>
/// Overview of ongoing pregnancy
/// </summary>
public class PregnancyInfoDataModel
{
    /// <summary>
    /// Current week of pregnancy
    /// </summary>
    public string WeekOfPregnancy { get; set; }

    /// <summary>
    /// Expected due date
    /// </summary>
    public DateTime ExpectedBirthDate { get; set; }

    /// <summary>
    /// Number of days there are left until due date
    /// </summary>
    public int RemainingDays { get; set; }
}
