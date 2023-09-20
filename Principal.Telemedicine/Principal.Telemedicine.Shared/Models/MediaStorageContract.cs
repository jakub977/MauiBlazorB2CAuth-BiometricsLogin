using System.Runtime.Serialization;

namespace Principal.Telemedicine.Shared.Models;

/// <summary>
/// Data contract derived from MediaStorage.cs
/// </summary>
[DataContract]
public class MediaStorageContract
{
    /// <summary>
    /// Primary identifier of a multimedia
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Binary data of a multimedia
    /// </summary>
    public byte[] Data { get; set; } = null!;

    /// <summary>
    /// Binary thumbnail of a multimedia
    /// </summary>
    public byte[]? Thumbnail { get; set; }

}
