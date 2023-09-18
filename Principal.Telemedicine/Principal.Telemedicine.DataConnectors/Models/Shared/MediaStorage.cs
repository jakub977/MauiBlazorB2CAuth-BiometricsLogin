using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Principal.Telemedicine.DataConnectors.Models.Shared;

/// <summary>
/// Table of binary data of multimedia
/// </summary>
[Table("MediaStorage")]
public partial class MediaStorage
{
    /// <summary>
    /// Primary identifier of a multimedia
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Binary data of a multimedia
    /// </summary>
    public byte[] Data { get; set; } = null!;

    public byte[]? Thumbnail { get; set; }

    [InverseProperty("MediaStorage")]
    public virtual ICollection<Picture> Pictures { get; set; } = new List<Picture>();
}
