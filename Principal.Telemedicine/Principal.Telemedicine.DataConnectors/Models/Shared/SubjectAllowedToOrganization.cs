using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Principal.Telemedicine.DataConnectors.Models.Shared;

/// <summary>
/// Table of subjects (modules) allowed to organization
/// </summary>
[Table("SubjectAllowedToOrganization")]
public partial class SubjectAllowedToOrganization
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
    [Column(TypeName = "datetime")]
    public DateTime? AllowedFromDateUtc { get; set; }

    /// <summary>
    /// Date to which subject (module) is allowed to organization, using coordinated universal time
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime? AllowedToDateUtc { get; set; }

    [ForeignKey("CreatedByCustomerId")]
    [InverseProperty("SubjectAllowedToOrganizationCreatedByCustomers")]
    public virtual Customer CreatedByCustomer { get; set; } = null!;

    [ForeignKey("OrganizationId")]
    [InverseProperty("SubjectAllowedToOrganizations")]
    public virtual Organization Organization { get; set; } = null!;

    [ForeignKey("SubjectId")]
    [InverseProperty("SubjectAllowedToOrganizations")]
    public virtual Subject Subject { get; set; } = null!;

    [InverseProperty("SubjectAllowedToOrganization")]
    public virtual ICollection<SubjectAllowedToProvider> SubjectAllowedToProviders { get; set; } = new List<SubjectAllowedToProvider>();

    [ForeignKey("UpdatedByCustomerId")]
    [InverseProperty("SubjectAllowedToOrganizationUpdatedByCustomers")]
    public virtual Customer? UpdatedByCustomer { get; set; }
}
