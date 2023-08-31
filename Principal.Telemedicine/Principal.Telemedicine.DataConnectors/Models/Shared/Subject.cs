using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Principal.Telemedicine.DataConnectors.Models.Shared;

/// <summary>
/// Table of subjects in the application
/// </summary>
[Table("Subject")]
public partial class Subject
{
    /// <summary>
    /// Primary identifier of a subject
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Bit identifier if a subject is active
    /// </summary>
    [Required]
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
    [Column(TypeName = "datetime")]
    public DateTime CreatedDateUtc { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who updates a subject
    /// </summary>
    public int? UpdatedByCustomerId { get; set; }

    /// <summary>
    /// Date of subject update, using coordinated universal time
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime? UpdateDateUtc { get; set; }

    /// <summary>
    /// Link to dbo.SubjectType as a type of a subject
    /// </summary>
    public int SubjectTypeId { get; set; }

    /// <summary>
    /// Name of a subject
    /// </summary>
    [StringLength(200)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// System name of a subject
    /// </summary>
    [StringLength(200)]
    [Unicode(false)]
    public string SystemName { get; set; } = null!;

    /// <summary>
    /// Link to dbo.Subject as a parent subject of subject, used for subject hierarchy
    /// </summary>
    public int? ParentSubjectId { get; set; }

    /// <summary>
    /// Name of subject icon from available web icon set (e.g. Font Awesome)
    /// </summary>
    [StringLength(100)]
    [Unicode(false)]
    public string? IconName { get; set; }

    [ForeignKey("CreatedByCustomerId")]
    [InverseProperty("SubjectCreatedByCustomers")]
    public virtual Customer CreatedByCustomer { get; set; } = null!;

    [InverseProperty("ParentSubject")]
    public virtual ICollection<Subject> InverseParentSubject { get; set; } = new List<Subject>();

    [ForeignKey("ParentSubjectId")]
    [InverseProperty("InverseParentSubject")]
    public virtual Subject? ParentSubject { get; set; }

    [InverseProperty("Subject")]
    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();

    [InverseProperty("Subject")]
    public virtual ICollection<SubjectAllowedToOrganization> SubjectAllowedToOrganizations { get; set; } = new List<SubjectAllowedToOrganization>();

    [ForeignKey("SubjectTypeId")]
    [InverseProperty("Subjects")]
    public virtual SubjectType SubjectType { get; set; } = null!;

    [ForeignKey("UpdatedByCustomerId")]
    [InverseProperty("SubjectUpdatedByCustomers")]
    public virtual Customer? UpdatedByCustomer { get; set; }
}
