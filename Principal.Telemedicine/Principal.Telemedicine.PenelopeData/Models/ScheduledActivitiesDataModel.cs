using System.Text.Json.Serialization;

namespace Principal.Telemedicine.PenelopeData.Models;

/// <summary>
/// Data model of patients scheduled activities - specific for Penelope.
/// </summary>
public class ScheduledActivitiesDataModel
{
    /// <summary>
    /// Unique identifier of activity.
    /// </summary>
    [JsonPropertyName("ActivityUniqueId")]
    public Guid ActivityUniqueId { get; set; }

    /// <summary>
    /// Date time of scheduled activity, using coordinated universal time
    /// </summary>
    [JsonPropertyName("ActivityDate")]
    public DateTime ActivityDate { get; set; }

    /// <summary>
    /// Type of scheduled activity
    /// </summary>
    [JsonPropertyName("ActivityType")]
    public string? ActivityType { get; set; }

    /// <summary>
    /// Code of activity
    /// </summary>
    [JsonPropertyName("ActivityCode")]
    public string? ActivityCode { get; set; }

    /// <summary>
    /// Body of notifitication related to the activity
    /// </summary>
    [JsonPropertyName("Notification")]
    public string? Notification { get; set; }

    /// <summary>
    /// Icon of activity
    /// </summary>
    [JsonPropertyName("Icon")]
    public string? Icon { get; set; }

    /// <summary>
    /// Bit identifier if scheduled activity is completed
    /// </summary>
    [JsonPropertyName("IsCompleted")]
    public int? IsCompleted { get; set; }

    /// <summary>
    /// Date time of scheduled activity update, using coordinated universal time
    /// </summary>
    [JsonPropertyName("LastUpdatedDateUtc")]
    public DateTime LastUpdatedDateUtc { get; set; }
}
