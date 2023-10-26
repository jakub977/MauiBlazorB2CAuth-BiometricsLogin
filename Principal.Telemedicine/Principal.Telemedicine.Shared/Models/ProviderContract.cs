using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

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
    public int Id { get; set; }

    /// <summary>
    /// Bit identifier if a provider is active
    /// </summary>
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
    public DateTime CreatedDateUtc { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who updates a provider
    /// </summary>
    public int? UpdatedByCustomerId { get; set; }

    /// <summary>
    /// Date of provider update, using coordinated universal time
    /// </summary>
    public DateTime? UpdateDateUtc { get; set; }

    /// <summary>
    /// Link to dbo.Organization as a parent organization
    /// </summary>
    public int OrganizationId { get; set; }

    /// <summary>
    /// Name of a provider
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Address line of a provider (street, land registry number or house number, city)
    /// </summary>
    public string? AddressLine { get; set; }

    /// <summary>
    /// Postal code of a provider
    /// </summary>
    public string? PostalCode { get; set; }

    /// <summary>
    /// Link to dbo.Picture as a photo of a provider
    /// </summary>
    public int? PictureId { get; set; }

    [DataMember]
    [JsonPropertyName("pictureObject")]
    public virtual PictureContract? Picture { get; set; }

    /// <summary>
    /// Identification number of an organization
    /// </summary>
    public string? IdentificationNumber { get; set; }

    /// <summary>
    /// Tax identification number of an organization
    /// </summary>
    public string? TaxIdentificationNumber { get; set; }

    /// <summary>
    /// Street related to address line of a provider
    /// </summary>
    public string? Street { get; set; }

    /// <summary>
    /// Link to dbo.AddressCity as a city related to provider
    /// </summary>
    public int? CityId { get; set; }

    /// <summary>
    /// Link to dbo.AddressCity as a city related to provider
    /// </summary>
    [JsonPropertyName("cityObject")]
    public virtual AddressCityContract? City { get; set; }

    /// <summary>
    /// Count of EffectiveUsers related to provider
    /// </summary>
    [JsonPropertyName("adminUsers")]
    public virtual ICollection<EffectiveUserContract> EffectiveUsers { get; set; } = new List<EffectiveUserContract>();

     /// <summary>
     /// Link to dbo.Organization as a parent organization
     /// </summary>
     [JsonPropertyName("organizationObject")]
    public virtual OrganizationContract? Organization { get; set; }

    /// <summary>
    /// Link to dbo.Picture as a photo of a provider
    /// </summary>
    [JsonPropertyName("pictureObject")]
    public virtual PictureContract? Picture { get; set; }

    /// <summary>
    /// Inverse collection of SubjectAllowedToProvider
    /// </summary>
    [JsonPropertyName("allowedSubjects")]
    public virtual ICollection<SubjectAllowedToProviderContract> SubjectAllowedToProviders { get; set; } = new List<SubjectAllowedToProviderContract>();

    /// <summary>
    /// Inverse collection of Permission
    /// </summary>
    [JsonPropertyName("permissionObject")]
    public virtual ICollection<PermissionContract> Permission { get; set; } = new List<PermissionContract>();

    /// <summary>
    /// Collection of providers EffectiveUsers
    /// </summary>
    [DataMember]
    [JsonPropertyName("EffectiveUsers")]
    public ICollection<EffectiveUserProviderContract> EffectiveUserProviderUsers { get; set; } = new List<EffectiveUserProviderContract>();

}