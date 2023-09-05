using System.Runtime.Serialization;

namespace Principal.Telemedicine.Shared.Models;

/// <summary>
/// Data contract derived from Organization.cs
/// </summary>
[DataContract]
public class OrganizationContract
{
    /// <summary>
    /// Primary identifier of an organization
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Bit identifier if an organization is active
    /// </summary>
    public bool? Active { get; set; }

    /// <summary>
    /// Bit identifier if an organization is deleted
    /// </summary>
    public bool Deleted { get; set; }

    /// <summary>
    /// Date of organization creation, using coordinated universal time
    /// </summary>
    public DateTime CreatedDateUtc { get; set; }

    /// <summary>
    /// Date of organization update, using coordinated universal time
    /// </summary>
    public DateTime? UpdateDateUtc { get; set; }

    /// <summary>
    /// Name of an organization
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Identification number of an organization
    /// </summary>
    public string IdentificationNumber { get; set; } = null!;

    /// <summary>
    /// Tax identification number of an organization
    /// </summary>
    public string TaxIdentificationNumber { get; set; } = null!;

    /// <summary>
    /// Address line of an organization (street, land registry number or house number, city)
    /// </summary>
    public string AddressLine { get; set; } = null!;

    /// <summary>
    /// Postal code of an organization
    /// </summary>
    public string PostalCode { get; set; } = null!;

    //public virtual ICollection<UserContract> Customers { get; set; } = new List<UserContract>();

    //public virtual ICollection<ProviderContract> Providers { get; set; } = new List<ProviderContract>();

   // public virtual ICollection<RoleContract> Roles { get; set; } = new List<RoleContract>();

    //[InverseProperty("Organization")]
    //public virtual ICollection<SubjectAllowedToOrganizationContract> SubjectAllowedToOrganizations { get; set; } = new List<SubjectAllowedToOrganizationContract>();

}
