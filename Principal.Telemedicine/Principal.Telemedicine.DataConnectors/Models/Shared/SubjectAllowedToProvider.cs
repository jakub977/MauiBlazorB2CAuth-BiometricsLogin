using Principal.Telemedicine.Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Principal.Telemedicine.DataConnectors.Models.Shared;

/// <summary>
/// Table of subjects (modules) allowed to organization
/// </summary>
[Table("SubjectAllowedToProvider")]
public partial class SubjectAllowedToProvider
{
    /// <summary>
    /// Primary identifier of a subject
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Bit identifier if subject is active
    /// </summary>
    [Required]
    public bool? Active { get; set; }

    /// <summary>
    /// Bit identifier if subject is deleted
    /// </summary>
    public bool Deleted { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who creates subject
    /// </summary>
    public int CreatedByCustomerId { get; set; }

    /// <summary>
    /// Date of subject creation, using coordinated universal time
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime CreatedDateUtc { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who updates subject
    /// </summary>
    public int? UpdatedByCustomerId { get; set; }

    /// <summary>
    /// Date of subject update, using coordinated universal time
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime? UpdateDateUtc { get; set; }

    /// <summary>
    /// Link to dbo.SubjectAllowedToOrganization as set of subjects (modules) which are allowed to organization and from which subjects (modules) can be allowed to specific provider
    /// </summary>
    public int SubjectAllowedToOrganizationId { get; set; }

    /// <summary>
    /// Link to dbo.Provider as an organization to which subjects are allowed
    /// </summary>
    public int ProviderId { get; set; }

    /// <summary>
    /// Date from which subject (module) is allowed to organization, using coordinated universal time
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime? AllowedFromDateUtc { get; set; }

    /// <summary>
    /// Date to which subject (module) is allowed to organization, using coordinated universal time
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime? AllowedToDateUtc { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who creates a subject allowed to provider
    /// </summary>
    [ForeignKey("CreatedByCustomerId")]
    [InverseProperty("SubjectAllowedToProviderCreatedByCustomers")]
    public virtual Customer CreatedByCustomer { get; set; } = null!;

    /// <summary>
    /// Link to dbo.Provider as an organization to which subjects are allowed
    /// </summary>
    [ForeignKey("ProviderId")]
    [InverseProperty("SubjectAllowedToProviders")]
    public virtual Provider Provider { get; set; } = null!;

    /// <summary>
    /// Link to dbo.SubjectAllowedToOrganization as set of subjects (modules) which are allowed to organization and from which subjects (modules) can be allowed to specific provider
    /// </summary>
    [ForeignKey("SubjectAllowedToOrganizationId")]
    [InverseProperty("SubjectAllowedToProviders")]
    public virtual SubjectAllowedToOrganization SubjectAllowedToOrganization { get; set; } = null!;

    /// <summary>
    /// Link to dbo.Customer as an user who updates a subject allowed to provider
    /// </summary>
    [ForeignKey("UpdatedByCustomerId")]
    [InverseProperty("SubjectAllowedToProviderUpdatedByCustomers")]
    public virtual Customer? UpdatedByCustomer { get; set; }

    /// <summary>
    /// Vrátí SubjectAllowedToProviderContract z SubjectAllowedToProvider
    /// </summary>
    /// <returns>SubjectAllowedToProviderContract</returns>
    public SubjectAllowedToProviderContract ConvertToSubjectAllowedToProviderContract()
    {
        SubjectAllowedToProviderContract data = new SubjectAllowedToProviderContract();

        data.Active = Active.GetValueOrDefault();
        data.AllowedFromDateUtc = AllowedFromDateUtc;
        data.AllowedToDateUtc = AllowedToDateUtc;
        data.CreatedByCustomerId = CreatedByCustomerId;
        data.CreatedDateUtc = CreatedDateUtc;
        data.Deleted = Deleted;
        data.Id = Id;
        data.ProviderId = ProviderId;
        data.SubjectAllowedToOrganizationId = SubjectAllowedToOrganizationId;
        data.SubjectAllowedToOrganizationObject = SubjectAllowedToOrganization.ConvertToSubjectAllowedToOrganizationContract();
        data.UpdateDateUtc = UpdateDateUtc;
        data.UpdatedByCustomerId = UpdatedByCustomerId;

        return data;
    }
}
