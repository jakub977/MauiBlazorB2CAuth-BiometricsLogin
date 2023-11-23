using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Principal.Telemedicine.DataConnectors.Models.Shared;

/// <summary>
/// Table content type of a notification/message.
/// </summary>
[Table("AppMessageContentType")]
public partial class AppMessageContentType
{
    /// <summary>
    /// Primary identifier of a notification/message content type
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Bit identifier if a content type is active
    /// </summary>
    [Required]
    public bool? Active { get; set; }

    /// <summary>
    /// Notification/message content type code
    /// </summary>
    [StringLength(50)]
    public string ContentCode { get; set; } = null!;

     /// <summary>
     /// Name of a content type
     /// </summary>
    [StringLength(255)]
    public string ContentName { get; set; } = null!;

    /// <summary>
    /// Inverse collection of AppMessageAdditionalAttribute
    /// </summary>
    [InverseProperty("AppMessageContentType")]
    public virtual ICollection<AppMessageAdditionalAttribute> AppMessageAdditionalAttributes { get; set; } = new List<AppMessageAdditionalAttribute>();

    /// <summary>
    /// Inverse collection of AppMessageSentLog
    /// </summary>
    [InverseProperty("AppMessageContentType")]
    public virtual ICollection<AppMessageSentLog> AppMessageSentLogs { get; set; } = new List<AppMessageSentLog>();

    /// <summary>
    /// Inverse collection of AppMessageTemplate
    /// </summary>
    [InverseProperty("AppMessageContentType")]
    public virtual ICollection<AppMessageTemplate> AppMessageTemplates { get; set; } = new List<AppMessageTemplate>();
}
