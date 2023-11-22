using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Principal.Telemedicine.DataConnectors.Models.Shared;


/// <summary>
/// Table of additional atributes of a notification/message.
/// </summary>
[Table("AppMessageAdditionalAttribute")]
public partial class AppMessageAdditionalAttribute
{
    /// <summary>
    /// Primary identifier of an additional attribute
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Content of an additional attribute
    /// </summary>
    [StringLength(255)]
    public string AttributeContent { get; set; } = null!;

    /// <summary>
    /// Link to dbo.AppMessageContentType as an identifier of the notification/message content
    /// </summary>
    public int AppMessageContentTypeId { get; set; }

    /// <summary>
    /// Link to dbo.AppMessageContentType as an identifier of the notification/message content
    /// </summary>
    [ForeignKey("AppMessageContentTypeId")]
    [InverseProperty("AppMessageAdditionalAttributes")]
    public virtual AppMessageContentType AppMessageContentType { get; set; } = null!;
}
