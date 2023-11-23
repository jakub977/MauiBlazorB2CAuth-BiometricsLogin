using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Principal.Telemedicine.DataConnectors.Models.Shared;

/// <summary>
/// Table of notification/message template.
/// </summary>
[Table("AppMessageTemplate")]
public partial class AppMessageTemplate
{
    /// <summary>
    /// Primary identifier of a template
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Title of a notification/message
    /// </summary>
    [StringLength(255)]
    public string? Title { get; set; }

    /// <summary>
    /// Content of a notification/message
    /// </summary>
    public string? Body { get; set; }

    /// <summary>
    /// Link to dbo.Language
    /// </summary>
    public int LanguageId { get; set; }

    /// <summary>
    /// Link to dbo.AppMessageContentType as a content type of the notification/message
    /// </summary>
    public int AppMessageContentTypeId { get; set; }

    /// <summary>
    /// Link to dbo.AppMessageContentType as a content type of the notification/message
    /// </summary>
    [ForeignKey("AppMessageContentTypeId")]
    [InverseProperty("AppMessageTemplates")]
    public virtual AppMessageContentType AppMessageContentType { get; set; } = null!;
}
