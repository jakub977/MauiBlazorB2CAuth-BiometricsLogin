namespace Principal.Telemedicine.PenelopeData.Models;

/// <summary>
/// Data model of the patient's planned activities.
/// </summary>
public class ScheduledActivityDataModel
{
    /// <summary>
    /// Unique activity identifier from the planned activities
    /// </summary>
    public string ActivityUniqueId { get; set; }

    /// <summary>
    /// Bit identifier if scheduled activity is completed
    /// </summary>
    public bool IsCompleted { get; set; }

    /// <summary>
    /// Date and time the activity was completed
    /// </summary>
    public DateTime DateOfCompletion { get; set; }
}

