using System.Runtime.Serialization;

namespace Principal.Telemedicine.Shared.Models;

/// <summary>
/// Data contract derived from SubjectAllowedToOrganization.cs
/// </summary>
[DataContract]
public class SubjectAllowedToOrganizationContract
{
    /// <summary>
    /// Primary identifier of a subject
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Bit identifier if subject is active
    /// </summary>
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
    public DateTime CreatedDateUtc { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who updates subject
    /// </summary>
    public int? UpdatedByCustomerId { get; set; }

    /// <summary>
    /// Date of subject update, using coordinated universal time
    /// </summary>
    public DateTime? UpdateDateUtc { get; set; }

    /// <summary>
    /// Link to dbo.Subject as a specific subject (module) which is allowed to organization
    /// </summary>
    public int SubjectId { get; set; }

    /// <summary>
    /// Link to dbo.Organization as an organization to which subjects are allowed
    /// </summary>
    public int OrganizationId { get; set; }

    /// <summary>
    /// Date from which subject (module) is allowed to organization, using coordinated universal time
    /// </summary>
    public DateTime? AllowedFromDateUtc { get; set; }

    /// <summary>
    /// Date to which subject (module) is allowed to organization, using coordinated universal time
    /// </summary>
    public DateTime? AllowedToDateUtc { get; set; }


    //public virtual UserContract CreatedByCustomer { get; set; } = null!;

    //public virtual OrganizationContract Organization { get; set; } = null!;

    //public virtual SubjectContract Subject { get; set; } = null!;

    //public virtual ICollection<SubjectAllowedToProviderContract> SubjectAllowedToProviders { get; set; } = new List<SubjectAllowedToProviderContract>();

    //public virtual UserContract? UpdatedByCustomer { get; set; }
}
