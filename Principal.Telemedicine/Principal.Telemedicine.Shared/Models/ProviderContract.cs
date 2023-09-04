using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Principal.Telemedicine.Shared.Models;

/// <summary>
/// Data contract derived from Provider.cs
/// </summary>
[DataContract]
public class ProviderContract
{
    /// <summary>
    /// Primary identifier of a provider
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Bit identifier if a provider is active
    /// </summary>
    [Required]
    public bool? Active { get; set; }

    /// <summary>
    /// Bit identifier if a provider is deleted
    /// </summary>
    public bool Deleted { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who creates a provider
    /// </summary>
    public int CreatedByCustomerId { get; set; }

    /// <summary>
    /// Date of provider creation, using coordinated universal time
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime CreatedDateUtc { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who updates a provider
    /// </summary>
    public int? UpdatedByCustomerId { get; set; }

    /// <summary>
    /// Date of provider update, using coordinated universal time
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime? UpdateDateUtc { get; set; }

    /// <summary>
    /// Link to dbo.Organization as a parent organization
    /// </summary>
    public int OrganizationId { get; set; }

    /// <summary>
    /// Name of a provider
    /// </summary>
    [StringLength(200)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Address line of a provider (street, land registry number or house number, city)
    /// </summary>
    [StringLength(200)]
    public string? AddressLine { get; set; }

    /// <summary>
    /// Postal code of a provider
    /// </summary>
    [StringLength(50)]
    public string? PostalCode { get; set; }

    /// <summary>
    /// Link to dbo.Picture as a photo of a provider
    /// </summary>
    public int? PictureId { get; set; }

    /// <summary>
    /// Identification number of an organization
    /// </summary>
    [StringLength(20)]
    public string? IdentificationNumber { get; set; }

    /// <summary>
    /// Tax identification number of an organization
    /// </summary>
    [StringLength(20)]
    public string? TaxIdentificationNumber { get; set; }

    [StringLength(100)]
    public string? Street { get; set; }

    public int? CityId { get; set; }

    [ForeignKey("CityId")]
    [InverseProperty("Providers")]
    public virtual AddressCityContract? City { get; set; }

    [ForeignKey("CreatedByCustomerId")]
    [InverseProperty("ProviderCreatedByCustomers")]
    public virtual UserContract CreatedByCustomer { get; set; } = null!;

    [InverseProperty("CreatedByProvider")]
    public virtual ICollection<UserContract> Customers { get; set; } = new List<UserContract>();

    [InverseProperty("Provider")]
    public virtual ICollection<EffectiveUserContract> EffectiveUsers { get; set; } = new List<EffectiveUserContract>();

    [InverseProperty("Provider")]
    public virtual ICollection<GroupContract> Groups { get; set; } = new List<GroupContract>();

    [ForeignKey("OrganizationId")]
    [InverseProperty("Providers")]
    public virtual OrganizationContract Organization { get; set; } = null!;

    [ForeignKey("PictureId")]
    [InverseProperty("Providers")]
    public virtual PictureContract? Picture { get; set; }

    [InverseProperty("Provider")]
    public virtual ICollection<RoleContract> Roles { get; set; } = new List<RoleContract>();

    [InverseProperty("Provider")]
    public virtual ICollection<SubjectAllowedToProviderContract> SubjectAllowedToProviders { get; set; } = new List<SubjectAllowedToProviderContract>();

    [ForeignKey("UpdatedByCustomerId")]
    [InverseProperty("ProviderUpdatedByCustomers")]
    public virtual UserContract? UpdatedByCustomer { get; set; }

    [InverseProperty("Provider")]
    public virtual ICollection<UserPermissionContract> UserPermissions { get; set; } = new List<UserPermissionContract>();
}
