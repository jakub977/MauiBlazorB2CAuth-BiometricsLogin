namespace Principal.Telemedicine.PenelopeData.Models;

/// <summary>
/// Data model of patients scheduled activities - specific for Penelope.
/// </summary>
public class ScheduledActivitiesDataModel
{
    /// <summary>
    /// Unique identifier of activity.
    /// </summary>
    public Guid ActivityUniqueId { get; set; }

    /// <summary>
    /// Date time of scheduled activity, using coordinated universal time
    /// </summary>
    public DateTime ActivityDate { get; set; }

    /// <summary>
    /// Type of scheduled activity
    /// </summary>
    public string? ActivityType { get; set; }

    /// <summary>
    /// Code of activity
    /// </summary>
    public string? ActivityCode { get; set; }

    /// <summary>
    /// Body of notifitication related to the activity
    /// </summary>
    public string? Notification { get; set; }

    /// <summary>
    /// Icon of activity
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// Bit identifier if scheduled activity is completed
    /// </summary>
    public int? IsCompleted { get; set; }

    /// <summary>
    /// Date time of scheduled activity update, using coordinated universal time
    /// </summary>
    public DateTime LastUpdatedDateUtc { get; set; }
}
