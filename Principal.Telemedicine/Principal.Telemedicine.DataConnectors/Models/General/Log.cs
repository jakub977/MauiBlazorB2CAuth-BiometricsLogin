using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Principal.Telemedicine.DataConnectors.Models.General;

[Table("Log")]
public partial class Log
{
    [Key]
    public int Id { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? FriendlyTopic { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Source { get; set; }

    [StringLength(4000)]
    [Unicode(false)]
    public string? ShortMessage { get; set; }

    [Unicode(false)]
    public string? FullMessage { get; set; }

    [Unicode(false)]
    public string? AdditionalInfo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedDateUtc { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? InternalCommunicationId { get; set; }

    public int? GroupItem { get; set; }

    public int? TransformationPagingGroupItem { get; set; }

    public int? SourceAzureComponentTypeId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Environment { get; set; }
}
