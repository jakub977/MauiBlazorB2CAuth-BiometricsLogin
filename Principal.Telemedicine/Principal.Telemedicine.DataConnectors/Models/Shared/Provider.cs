using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Principal.Telemedicine.DataConnectors.Models.Shared;

/// <summary>
/// Table of providers, second level in organization structure
/// </summary>
[Table("Provider")]
public partial class Provider
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
    [DataMember]
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
    [Unicode(false)]
    public string? PostalCode { get; set; }

    /// <summary>
    /// Link to dbo.Picture as a photo of a provider
    /// </summary>
    public int? PictureId { get; set; }

    /// <summary>
    /// Identification number of an organization
    /// </summary>
    [StringLength(20)]
    [Unicode(false)]
    public string? IdentificationNumber { get; set; }

    /// <summary>
    /// Tax identification number of an organization
    /// </summary>
    [StringLength(20)]
    [Unicode(false)]
    public string? TaxIdentificationNumber { get; set; }

    /// <summary>
    /// Street related to address line of a provider
    /// </summary>
    [StringLength(100)]
    [Unicode(false)]
    public string? Street { get; set; }

    /// <summary>
    /// Link to dbo.AddressCity as a city related to provider
    /// </summary>
    public int? CityId { get; set; }

    /// <summary>
    /// Link to dbo.AddressCity as a city related to provider
    /// </summary>
    [ForeignKey("CityId")]
    [InverseProperty("Providers")]
    public virtual AddressCity? City { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who creates a provider
    /// </summary>
    [ForeignKey("CreatedByCustomerId")]
    [InverseProperty("ProviderCreatedByCustomers")]
    public virtual Customer CreatedByCustomer { get; set; } = null!;

    /// <summary>
    /// Inverse collection of Customers to whom provider relates
    /// </summary>
    [InverseProperty("CreatedByProvider")]
    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    /// <summary>
    /// Inverse collection of EffectiveUsers to whom provider relates
    /// </summary>
    [InverseProperty("Provider")]
    public virtual ICollection<EffectiveUser> EffectiveUsers { get; set; } = new List<EffectiveUser>();

    /// <summary>
    /// Inverse collection of Groups to whom provider relates
    /// </summary>
    [InverseProperty("Provider")]
    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();

    /// <summary>
    /// Link to dbo.Organization as a parent organization
    /// </summary>
    [ForeignKey("OrganizationId")]
    [InverseProperty("Providers")]
    public virtual Organization Organization { get; set; } = null!;

    /// <summary>
    /// Link to dbo.Picture as a photo of a provider
    /// </summary>
    [ForeignKey("PictureId")]
    [InverseProperty("Providers")]
    public virtual Picture? Picture { get; set; }

    /// <summary>
    /// Inverse collection of Roles to whom provider relates
    /// </summary>
    [InverseProperty("Provider")]
    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();

    /// <summary>
    /// Inverse collection of SubjectAllowedToProvider
    /// </summary>
    [InverseProperty("Provider")]
    public virtual ICollection<SubjectAllowedToProvider> SubjectAllowedToProviders { get; set; } = new List<SubjectAllowedToProvider>();

    /// <summary>
    /// Link to dbo.Customer as an user who updates a provider
    /// </summary>
    [ForeignKey("UpdatedByCustomerId")]
    [InverseProperty("ProviderUpdatedByCustomers")]
    public virtual Customer? UpdatedByCustomer { get; set; }

    /// <summary>
    /// Inverse collection of UserPermissions related to provider
    /// </summary>
    [InverseProperty("Provider")]
    public virtual ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();
}
