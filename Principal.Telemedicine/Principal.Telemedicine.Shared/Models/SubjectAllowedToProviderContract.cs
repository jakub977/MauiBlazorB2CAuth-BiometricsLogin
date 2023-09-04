using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Principal.Telemedicine.Shared.Models;

/// <summary>
/// Data contract derived from SubjectAllowedToProvider.cs
/// </summary>
[DataContract]
public class SubjectAllowedToProviderContract
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

    [ForeignKey("CreatedByCustomerId")]
    [InverseProperty("SubjectAllowedToProviderCreatedByCustomers")]
    public virtual UserContract CreatedByCustomer { get; set; } = null!;

    [ForeignKey("ProviderId")]
    [InverseProperty("SubjectAllowedToProviders")]
    public virtual ProviderContract Provider { get; set; } = null!;

    [ForeignKey("SubjectAllowedToOrganizationId")]
    [InverseProperty("SubjectAllowedToProviders")]
    public virtual SubjectAllowedToOrganizationContract SubjectAllowedToOrganization { get; set; } = null!;

    [ForeignKey("UpdatedByCustomerId")]
    [InverseProperty("SubjectAllowedToProviderUpdatedByCustomers")]
    public virtual UserContract? UpdatedByCustomer { get; set; }
}
