using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Principal.Telemedicine.Shared.Models;

/// <summary>
/// Data contract derived from Role.cs
/// </summary>
[DataContract]
public class RoleContract
{
    /// <summary>
    /// Primary identifier of a role
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Bit identifier if role is active
    /// </summary>
    [Required]
    public bool? Active { get; set; }

    /// <summary>
    /// Bit identifier if role is deleted
    /// </summary>
    public bool Deleted { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who creates role
    /// </summary>
    public int CreatedByCustomerId { get; set; }

    /// <summary>
    /// Date of role creation, using coordinated universal time
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime CreatedDateUtc { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who updates role
    /// </summary>
    public int? UpdatedByCustomerId { get; set; }

    /// <summary>
    /// Date of role update, using coordinated universal time
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime? UpdateDateUtc { get; set; }

    /// <summary>
    /// Link to dbo.Organization as a parent organization
    /// </summary>
    public int? OrganizationId { get; set; }

    /// <summary>
    /// Link to dbo.Provider as a parent provider
    /// </summary>
    public int? ProviderId { get; set; }

    /// <summary>
    /// Bit identifier if role is global or custom (0 = global, 1 = custom). Global roles are created by super admins.
    /// </summary>
    public bool IsGlobal { get; set; }

    /// <summary>
    /// Name of a role
    /// </summary>
    [StringLength(200)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Detailed description of a role
    /// </summary>
    [StringLength(200)]
    public string? Description { get; set; }

    /// <summary>
    /// Link to dbo.Role as a parent role, i.e. reference to original role
    /// </summary>
    public int? ParentRoleId { get; set; }

    /// <summary>
    /// Link to dbo.RoleCategoryCombination as a combination of role category and its subcategory
    /// </summary>
    public int RoleCategoryCombinationId { get; set; }

    [ForeignKey("CreatedByCustomerId")]
    [InverseProperty("RoleCreatedByCustomers")]
    public virtual UserContract CreatedByCustomer { get; set; } = null!;

    [InverseProperty("ParentRole")]
    public virtual ICollection<RoleContract> InverseParentRole { get; set; } = new List<RoleContract>();

    [ForeignKey("OrganizationId")]
    [InverseProperty("Roles")]
    public virtual OrganizationContract? Organization { get; set; }

    [ForeignKey("ParentRoleId")]
    [InverseProperty("InverseParentRole")]
    public virtual RoleContract? ParentRole { get; set; }

    [ForeignKey("ProviderId")]
    [InverseProperty("Roles")]
    public virtual ProviderContract? Provider { get; set; }

    [ForeignKey("RoleCategoryCombinationId")]
    [InverseProperty("Roles")]
    public virtual RoleCategoryCombinationContract RoleCategoryCombination { get; set; } = null!;

    [InverseProperty("Role")]
    public virtual ICollection<RoleMemberContract> RoleMembers { get; set; } = new List<RoleMemberContract>();

    [InverseProperty("Role")]
    public virtual ICollection<RolePermissionContract> RolePermissions { get; set; } = new List<RolePermissionContract>();

    [ForeignKey("UpdatedByCustomerId")]
    [InverseProperty("RoleUpdatedByCustomers")]
    public virtual UserContract? UpdatedByCustomer { get; set; }
}
