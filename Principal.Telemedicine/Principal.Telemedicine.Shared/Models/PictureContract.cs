using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

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
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// 
    /// Mime type of a picture
    /// </summary>
    [StringLength(40)]
    public string MimeType { get; set; } = null!;

    /// <summary>
    /// SEO file name of a picture
    /// </summary>
    [StringLength(300)]
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
    [Column(TypeName = "datetime")]
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
    [Required]
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
    [Column(TypeName = "datetime")]
    public DateTime CreatedDateUtc { get; set; }

    /// <summary>
    /// Friendly name of a picture
    /// </summary>
    [StringLength(200)]
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
    [Column(TypeName = "decimal(10, 2)")]
    public decimal? OriginalSizeInkB { get; set; }

    /// <summary>
    /// Size of converted picture, in kB.
    /// </summary>
    [Column(TypeName = "decimal(10, 2)")]
    public decimal? ConvertedSizeInkB { get; set; }

    public int? ThumbnailWidth { get; set; }

    public int? ThumbnailHeight { get; set; }

    [ForeignKey("CreatedByCustomerId")]
    [InverseProperty("PictureCreatedByCustomers")]
    public virtual UserContract CreatedByCustomer { get; set; } = null!;

    [InverseProperty("Picture")]
    public virtual ICollection<UserContract> Customers { get; set; } = new List<UserContract>();

    [InverseProperty("Picture")]
    public virtual ICollection<GroupContract> Groups { get; set; } = new List<GroupContract>();

    [InverseProperty("Picture")]
    public virtual ICollection<ProviderContract> Providers { get; set; } = new List<ProviderContract>();

    [ForeignKey("UpdatedByCustomerId")]
    [InverseProperty("PictureUpdatedByCustomers")]
    public virtual UserContract? UpdatedByCustomer { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("PictureUsers")]
    public virtual UserContract? User { get; set; }
}
