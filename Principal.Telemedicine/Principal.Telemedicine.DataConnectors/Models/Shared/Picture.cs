using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Principal.Telemedicine.DataConnectors.Models.Shared;

/// <summary>
/// Table of pictures and their metadata.
/// </summary>
[Table("Picture")]
[Index("MediaStorageId", Name = "IX_MediaStorageId")]
[Index("UpdatedOnUtc", "IsTransient", Name = "IX_UpdatedOn_IsTransient")]
[Index("Id", Name = "_dta_index_Picture_5_1557580587__K1_4")]
public partial class Picture
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

    /// <summary>
    /// Width of thumbnail
    /// </summary>
    public int? ThumbnailWidth { get; set; }

    /// <summary>
    /// Height of thumbnail
    /// </summary>
    public int? ThumbnailHeight { get; set; }

    /// <summary>
    /// Link to dbo.MediaStorage as hex of a picture
    /// </summary>
    [ForeignKey("MediaStorageId")]
    [InverseProperty("Pictures")]
    public virtual MediaStorage? MediaStorage { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who creates a picture
    /// </summary>
    [ForeignKey("CreatedByCustomerId")]
    [InverseProperty("PictureCreatedByCustomers")]
    public virtual Customer CreatedByCustomer { get; set; } = null!;

    /// <summary>
    /// Inverse collection of Customers as an user to whom picture relates
    /// </summary>
    [InverseProperty("Picture")]
    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    /// <summary>
    /// Inverse collection of Groups as an group to whom picture relates
    /// </summary>
    [InverseProperty("Picture")]
    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();

    /// <summary>
    /// Inverse collection of Providers as an provider to whom picture relates
    /// </summary>
    [InverseProperty("Picture")]
    public virtual ICollection<Provider> Providers { get; set; } = new List<Provider>();

    /// <summary>
    /// Link to dbo.Customer as an user who updates a picture
    /// </summary>
    [ForeignKey("UpdatedByCustomerId")]
    [InverseProperty("PictureUpdatedByCustomers")]
    public virtual Customer? UpdatedByCustomer { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an users picture
    /// </summary>
    [ForeignKey("UserId")]
    [InverseProperty("PictureUsers")]
    public virtual Customer? User { get; set; }
}
