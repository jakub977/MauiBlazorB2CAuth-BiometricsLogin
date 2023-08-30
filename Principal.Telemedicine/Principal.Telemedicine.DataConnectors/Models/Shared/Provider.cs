using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

    [StringLength(100)]
    [Unicode(false)]
    public string? Street { get; set; }

    public int? CityId { get; set; }

    [ForeignKey("CityId")]
    [InverseProperty("Providers")]
    public virtual AddressCity? City { get; set; }

    [ForeignKey("CreatedByCustomerId")]
    [InverseProperty("ProviderCreatedByCustomers")]
    public virtual Customer CreatedByCustomer { get; set; } = null!;

    [InverseProperty("CreatedByProvider")]
    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    [InverseProperty("Provider")]
    public virtual ICollection<EffectiveUser> EffectiveUsers { get; set; } = new List<EffectiveUser>();

    [ForeignKey("OrganizationId")]
    [InverseProperty("Providers")]
    public virtual Organization Organization { get; set; } = null!;

    [ForeignKey("PictureId")]
    [InverseProperty("Providers")]
    public virtual Picture? Picture { get; set; }

    [InverseProperty("Provider")]
    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();

    [ForeignKey("UpdatedByCustomerId")]
    [InverseProperty("ProviderUpdatedByCustomers")]
    public virtual Customer? UpdatedByCustomer { get; set; }

    [InverseProperty("Provider")]
    public virtual ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();
}
