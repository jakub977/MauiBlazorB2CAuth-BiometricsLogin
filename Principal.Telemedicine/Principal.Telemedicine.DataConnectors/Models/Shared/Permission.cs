using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Principal.Telemedicine.DataConnectors.Models.Shared;

/// <summary>
/// Table of permissions on subjects
/// </summary>
[Table("Permission")]
public partial class Permission
{
    /// <summary>
    /// Primary identifier of a permission
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Bit identifier if a permission is active
    /// </summary>
    [Required]
    public bool? Active { get; set; }

    /// <summary>
    /// Bit identifier if a permission is deleted
    /// </summary>
    public bool Deleted { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who creates a permission
    /// </summary>
    public int CreatedByCustomerId { get; set; }

    /// <summary>
    /// Date of permission creation, using coordinated universal time
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime CreatedDateUtc { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who updates a permission
    /// </summary>
    public int? UpdatedByCustomerId { get; set; }

    /// <summary>
    /// Date of permission update, using coordinated universal time
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime? UpdateDateUtc { get; set; }

    /// <summary>
    /// Link to dbo.PermissionType as a permission type
    /// </summary>
    public int PermissionTypeId { get; set; }

    /// <summary>
    /// Link to dbo.Subject as a subject of the application
    /// </summary>
    public int SubjectId { get; set; }

    /// <summary>
    /// Link to dbo.PermissionCategory as a category of permission
    /// </summary>
    public int? PermissionCategoryId { get; set; }

    /// <summary>
    /// Link to dbo.Permission as a parent permission, used for hierarchy of permissions
    /// </summary>
    public int? ParentPermissionId { get; set; }

    /// <summary>
    /// Name of a permission
    /// </summary>
    [StringLength(200)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// System name of a permission
    /// </summary>
    [StringLength(200)]
    [Unicode(false)]
    public string SystemName { get; set; } = null!;

    /// <summary>
    /// Detailed description of a permission
    /// </summary>
    [StringLength(200)]
    public string? Description { get; set; }

    [ForeignKey("CreatedByCustomerId")]
    [InverseProperty("PermissionCreatedByCustomers")]
    public virtual Customer CreatedByCustomer { get; set; } = null!;

    [InverseProperty("ParentPermission")]
    public virtual ICollection<Permission> InverseParentPermission { get; set; } = new List<Permission>();

    [ForeignKey("ParentPermissionId")]
    [InverseProperty("InverseParentPermission")]
    public virtual Permission? ParentPermission { get; set; }

    [ForeignKey("PermissionCategoryId")]
    [InverseProperty("Permissions")]
    public virtual PermissionCategory? PermissionCategory { get; set; }

    [ForeignKey("PermissionTypeId")]
    [InverseProperty("Permissions")]
    public virtual PermissionType PermissionType { get; set; } = null!;

    [ForeignKey("SubjectId")]
    [InverseProperty("Permissions")]
    public virtual Subject Subject { get; set; } = null!;

    [ForeignKey("UpdatedByCustomerId")]
    [InverseProperty("PermissionUpdatedByCustomers")]
    public virtual Customer? UpdatedByCustomer { get; set; }

    [InverseProperty("Permission")]
    public virtual ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();
}
