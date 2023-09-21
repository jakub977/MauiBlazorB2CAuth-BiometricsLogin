using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Principal.Telemedicine.DataConnectors.Models.Shared;

/// <summary>
/// Table of permissions of roles
/// </summary>
[Table("RolePermission")]
public partial class RolePermission
{
    /// <summary>
    /// Primary identifier of a role permission
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Bit identifier if a role permission is active
    /// </summary>
    [Required]
    public bool? Active { get; set; }

    /// <summary>
    /// Bit identifier if a role permission is deleted
    /// </summary>
    public bool Deleted { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who creates a role permission
    /// </summary>
    public int CreatedByCustomerId { get; set; }

    /// <summary>
    /// Date of role permission creation, using coordinated universal time
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime CreatedDateUtc { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who updates a role permission
    /// </summary>
    public int? UpdatedByCustomerId { get; set; }

    /// <summary>
    /// Date of role permission update, using coordinated universal time
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime? UpdateDateUtc { get; set; }

    /// <summary>
    /// Link to dbo.Role as a role to which permission is assigned
    /// </summary>
    public int RoleId { get; set; }

    /// <summary>
    /// Link to dbo.Permission as a permission which is assigned to role
    /// </summary>
    public int PermissionId { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who creates a role permission
    /// </summary>
    [ForeignKey("CreatedByCustomerId")]
    [InverseProperty("RolePermissionCreatedByCustomers")]
    public virtual Customer CreatedByCustomer { get; set; } = null!;

    /// <summary>
    /// Link to dbo.Permission as a permission which is assigned to role
    /// </summary>
    [ForeignKey("PermissionId")]
    [InverseProperty("RolePermissions")]
    public virtual Permission Permission { get; set; } = null!;

    /// <summary>
    /// Link to dbo.Role as a role to which permission is assigned
    /// </summary>
    [ForeignKey("RoleId")]
    [InverseProperty("RolePermissions")]
    public virtual Role Role { get; set; } = null!;

    /// <summary>
    /// Link to dbo.Customer as an user who creates a role permission
    /// </summary>
    [ForeignKey("UpdatedByCustomerId")]
    [InverseProperty("RolePermissionUpdatedByCustomers")]
    public virtual Customer? UpdatedByCustomer { get; set; }
}
