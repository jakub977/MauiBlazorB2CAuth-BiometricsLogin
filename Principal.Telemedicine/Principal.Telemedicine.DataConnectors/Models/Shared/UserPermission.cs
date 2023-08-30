using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Principal.Telemedicine.DataConnectors.Models.Shared;

/// <summary>
/// Table of permissions which are explicitly granted or denied to users beyond permissions already granted based on assigned roles or memberships in any of organizational units
/// </summary>
[Table("UserPermission")]
public partial class UserPermission
{
    /// <summary>
    /// Primary identifier of an user permission
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Bit identifier if user permission is active
    /// </summary>
    [Required]
    public bool? Active { get; set; }

    /// <summary>
    /// Bit identifier if user permission is deleted
    /// </summary>
    public bool Deleted { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who creates user permission
    /// </summary>
    public int CreatedByCustomerId { get; set; }

    /// <summary>
    /// Date of user permission creation, using coordinated universal time
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime CreatedDateUtc { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who updates user permission
    /// </summary>
    public int? UpdatedByCustomerId { get; set; }

    /// <summary>
    /// Date of user permission update, using coordinated universal time
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime? UpdateDateUtc { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user to whom permission is granted or denied
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Link to dbo.Provider as a provider for which exception is valid
    /// </summary>
    public int? ProviderId { get; set; }

    /// <summary>
    /// Link to dbo.Permission as a permission which is granted or denied to user
    /// </summary>
    public int PermissionId { get; set; }

    /// <summary>
    /// Bit identifier if permission is denied (1) or granted (0) to user beyond the granted permission based on assigned role or membership in an organizational unit
    /// </summary>
    public bool IsDeniedPermission { get; set; }

    [ForeignKey("CreatedByCustomerId")]
    [InverseProperty("UserPermissionCreatedByCustomers")]
    public virtual Customer CreatedByCustomer { get; set; } = null!;

    [ForeignKey("PermissionId")]
    [InverseProperty("UserPermissions")]
    public virtual Permission Permission { get; set; } = null!;

    [ForeignKey("ProviderId")]
    [InverseProperty("UserPermissions")]
    public virtual Provider? Provider { get; set; }

    [ForeignKey("UpdatedByCustomerId")]
    [InverseProperty("UserPermissionUpdatedByCustomers")]
    public virtual Customer? UpdatedByCustomer { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("UserPermissionUsers")]
    public virtual Customer User { get; set; } = null!;
}
