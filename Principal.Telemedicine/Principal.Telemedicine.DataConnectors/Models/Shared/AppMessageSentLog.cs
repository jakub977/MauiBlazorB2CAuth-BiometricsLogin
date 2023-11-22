using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Principal.Telemedicine.DataConnectors.Models.Shared;

/// <summary>
/// Table of sent notifications/messages to FCM service.
/// </summary>
[Table("AppMessageSentLog")]
public partial class AppMessageSentLog
{
    /// <summary>
    /// Primary identifier of a log
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Link to dbo.Customer as a notificated user
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Date and time of notification/message processing
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime MessageSentDateUtc { get; set; }

    /// <summary>
    /// Link to dbo.AppMessageContentType as a content type of the notification/message
    /// </summary>
    public int AppMessageContentTypeId { get; set; }

    /// <summary>
    /// Id of notification/message provided by FCM
    /// </summary>
    [StringLength(100)]
    [Unicode(false)]
    public string MessageId { get; set; } = null!;

    /// <summary>
    /// Date and time of notification/message delivering
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime? MessageDeliveryDateUtc { get; set; }

    /// <summary>
    /// Link to dbo.AppMessageContentType as a content type of the notification/message
    /// </summary>
    [ForeignKey("AppMessageContentTypeId")]
    [InverseProperty("AppMessageSentLogs")]
    public virtual AppMessageContentType AppMessageContentType { get; set; } = null!;
}
