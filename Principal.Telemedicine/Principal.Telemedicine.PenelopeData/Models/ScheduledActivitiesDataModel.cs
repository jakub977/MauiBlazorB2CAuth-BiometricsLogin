namespace Principal.Telemedicine.PenelopeData.Models;

/// <summary>
/// Data model of patients scheduled activities - specific for Penelope.
/// </summary>
public class ScheduledActivitiesDataModel
{
    public Guid ActivityUniqueId { get; set; }
    public DateTime ActivityDate { get; set; }
    public string? ActivityType { get; set; }
    public string? ActivityCode { get; set; }
    public string? Notification { get; set; }
    public string? Icon { get; set; }
    public int? IsCompleted { get; set; }
    public DateTime LastUpdatedDateUtc { get; set; }
}
