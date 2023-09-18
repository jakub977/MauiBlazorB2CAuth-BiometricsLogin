using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Principal.Telemedicine.Shared.Models;

/// <summary>
/// Data contract derived from Picture.cs
/// </summary>
[DataContract]
public class PictureContract
{
    /// <summary>
    /// Primary identifier of a picture
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 
    /// Mime type of a picture
    /// </summary>
    public string MimeType { get; set; } = null!;

    /// <summary>
    /// SEO file name of a picture
    /// </summary>
    public string? SeoFilename { get; set; }

    /// <summary>
    /// Bit identifier if a picture is new
    /// </summary>
    public bool IsNew { get; set; }

    /// <summary>
    /// Bit identifier if a picture is transient
    /// </summary>
    public bool IsTransient { get; set; }

    /// <summary>
    /// Date of picture update, using coordinated universal time
    /// </summary>
    public DateTime? UpdatedOnUtc { get; set; }

    /// <summary>
    /// Link to dbo.MediaStorage as hex of a picture
    /// </summary>
    public int? MediaStorageId { get; set; }

    /// <summary>
    /// Width of a picture
    /// </summary>
    public int? Width { get; set; }

    /// <summary>
    /// Height of a picture
    /// </summary>
    public int? Height { get; set; }

    /// <summary>
    /// Bit identifier if a picture is active
    /// </summary>
    public bool? Active { get; set; }

    /// <summary>
    /// Bit identifier if a picture is deleted
    /// </summary>
    public bool Deleted { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who creates a picture
    /// </summary>
    public int CreatedByCustomerId { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who updates a picture
    /// </summary>
    public int? UpdatedByCustomerId { get; set; }

    /// <summary>
    /// Date of picture creation, using coordinated universal time
    /// </summary>
    public DateTime CreatedDateUtc { get; set; }

    /// <summary>
    /// Friendly name of a picture
    /// </summary>
    public string? FriendlyName { get; set; }

    /// <summary>
    /// Bit identifier if a picture is also public for another user
    /// </summary>
    public bool IsPublic { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user to whom picture relates. Used when column CreatedByCustomerId is different to this column: e.g. a doctor imports a picture of a patient (CreatedByCustomerId = doctor, UserId = patient)
    /// </summary>
    public int? UserId { get; set; }

    /// <summary>
    /// Link to dbo.DiseaseSymptomCategory as a category of disease symptom, using for ML
    /// </summary>
    public int? DiseaseSymptomCategoryId { get; set; }

    /// <summary>
    /// Bit identifier if a picture was successfully converted
    /// </summary>
    public bool IsConverted { get; set; }

    /// <summary>
    /// Size of original picture, in kB.
    /// </summary>
    public decimal? OriginalSizeInkB { get; set; }

    /// <summary>
    /// Size of converted picture, in kB.
    /// </summary>
    public decimal? ConvertedSizeInkB { get; set; }

    public int? ThumbnailWidth { get; set; }

    public int? ThumbnailHeight { get; set; }

    [JsonPropertyName("mediaStorageObject")]
    public virtual MediaStorageContract? MediaStorage { get; set; }
}
