using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Principal.Telemedicine.PenelopeData.Models;

/// <summary>
/// Overview of ongoing pregnancy
/// </summary>
public class PregnancyInfoDataModel
{
    /// <summary>
    /// Current week of pregnancy
    /// </summary>
    [JsonPropertyName("WeekOfPregnancy")]
    public string WeekOfPregnancy { get; set; }

    /// <summary>
    /// Expected due date
    /// </summary>
    [JsonPropertyName("ExpectedBirthDate")]
    public DateTime ExpectedBirthDate { get; set; }

    /// <summary>
    /// Number of days there are left until due date
    /// </summary>
    [JsonPropertyName("RemainingDays")]
    public int RemainingDays { get; set; }
}
