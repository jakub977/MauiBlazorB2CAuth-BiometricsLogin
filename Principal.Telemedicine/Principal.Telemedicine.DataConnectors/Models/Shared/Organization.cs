using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Principal.Telemedicine.Shared.Models;

namespace Principal.Telemedicine.DataConnectors.Models.Shared;

/// <summary>
/// Table of organizations, the highest level of organization hierarchy
/// </summary>
[Table("Organization")]
public partial class Organization
{
    /// <summary>
    /// Primary identifier of an organization
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Bit identifier if an organization is active
    /// </summary>
    [Required]
    public bool? Active { get; set; }

    /// <summary>
    /// Bit identifier if an organization is deleted
    /// </summary>
    public bool Deleted { get; set; }

    /// <summary>
    /// Date of organization creation, using coordinated universal time
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime CreatedDateUtc { get; set; }

    /// <summary>
    /// Date of organization update, using coordinated universal time
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime? UpdateDateUtc { get; set; }

    /// <summary>
    /// Name of an organization
    /// </summary>
    [StringLength(200)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Identification number of an organization
    /// </summary>
    [StringLength(20)]
    [Unicode(false)]
    public string IdentificationNumber { get; set; } = null!;

    /// <summary>
    /// Tax identification number of an organization
    /// </summary>
    [StringLength(20)]
    [Unicode(false)]
    public string TaxIdentificationNumber { get; set; } = null!;

    /// <summary>
    /// Address line of an organization (street, land registry number or house number, city)
    /// </summary>
    [StringLength(200)]
    public string AddressLine { get; set; } = null!;

    /// <summary>
    /// Postal code of an organization
    /// </summary>
    [StringLength(20)]
    [Unicode(false)]
    public string PostalCode { get; set; } = null!;

    /// <summary>
    /// Inverse collection of Customers with organization
    /// </summary>
    [InverseProperty("Organization")]
    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    /// <summary>
    /// Inverse collection of Providers of organization
    /// </summary>
    [InverseProperty("Organization")]
    public virtual ICollection<Provider> Providers { get; set; } = new List<Provider>();

    /// <summary>
    /// Inverse collection of Roles allowed to organization
    /// </summary>
    [InverseProperty("Organization")]
    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();

    /// <summary>
    /// Inverse collection of SubjectAllowedToOrganizations
    /// </summary>
    [InverseProperty("Organization")]
    public virtual ICollection<SubjectAllowedToOrganization> SubjectAllowedToOrganizations { get; set; } = new List<SubjectAllowedToOrganization>();

    /// <summary>
    /// Vrátí OrganizationContract z Organization
    /// </summary>
    /// <returns>OrganizationContract</returns>
    public OrganizationContract ConvertToOrganizationContract()
    {
        OrganizationContract data = new OrganizationContract();

        data.Active = Active.GetValueOrDefault();
        data.AddressLine = AddressLine;
        data.CreatedDateUtc = CreatedDateUtc;
        data.Deleted = Deleted;
        data.Id = Id;
        data.IdentificationNumber = IdentificationNumber;
        data.Name = Name;
        data.PostalCode = PostalCode;
        data.TaxIdentificationNumber = TaxIdentificationNumber;
        data.UpdateDateUtc = UpdateDateUtc;

        return data;
    }
}
