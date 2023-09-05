using System.Runtime.Serialization;

namespace Principal.Telemedicine.Shared.Models;

/// <summary>
/// Data contract derived from Subject.cs
/// </summary>
[DataContract]
public class SubjectContract
{
    /// <summary>
    /// Primary identifier of a subject
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Bit identifier if a subject is active
    /// </summary>
    public bool? Active { get; set; }

    /// <summary>
    /// Bit identifier if a subject is deleted
    /// </summary>
    public bool Deleted { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who creates a subject
    /// </summary>
    public int CreatedByCustomerId { get; set; }

    /// <summary>
    /// Date of subject creation, using coordinated universal time
    /// </summary>
    public DateTime CreatedDateUtc { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who updates a subject
    /// </summary>
    public int? UpdatedByCustomerId { get; set; }

    /// <summary>
    /// Date of subject update, using coordinated universal time
    /// </summary>
    public DateTime? UpdateDateUtc { get; set; }

    /// <summary>
    /// Link to dbo.SubjectType as a type of a subject
    /// </summary>
    public int SubjectTypeId { get; set; }

    /// <summary>
    /// Name of a subject
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// System name of a subject
    /// </summary>
    public string SystemName { get; set; } = null!;

    /// <summary>
    /// Link to dbo.Subject as a parent subject of subject, used for subject hierarchy
    /// </summary>
    public int? ParentSubjectId { get; set; }

    /// <summary>
    /// Name of subject icon from available web icon set (e.g. Font Awesome)
    /// </summary>
    public string? IconName { get; set; }

    //public virtual UserContract CreatedByCustomer { get; set; } = null!;

    //public virtual ICollection<SubjectContract> InverseParentSubject { get; set; } = new List<SubjectContract>();

    //public virtual SubjectContract? ParentSubject { get; set; }

    //public virtual ICollection<PermissionContract> Permissions { get; set; } = new List<PermissionContract>();

    //public virtual ICollection<SubjectAllowedToOrganizationContract> SubjectAllowedToOrganizations { get; set; } = new List<SubjectAllowedToOrganizationContract>();

    //public virtual SubjectTypeContract SubjectType { get; set; } = null!;

    //public virtual UserContract? UpdatedByCustomer { get; set; }
}
