using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Principal.Telemedicine.Shared.Models;

/// <summary>
/// Data contract derived from RolePermission.cs
/// </summary>
[DataContract]
public class RolePermissionContract
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

    [ForeignKey("CreatedByCustomerId")]
    [InverseProperty("RolePermissionCreatedByCustomers")]
    public virtual UserContract CreatedByCustomer { get; set; } = null!;

    [ForeignKey("PermissionId")]
    [InverseProperty("RolePermissions")]
    public virtual PermissionContract Permission { get; set; } = null!;

    [ForeignKey("RoleId")]
    [InverseProperty("RolePermissions")]
    public virtual RoleContract Role { get; set; } = null!;

    [ForeignKey("UpdatedByCustomerId")]
    [InverseProperty("RolePermissionUpdatedByCustomers")]
    public virtual UserContract? UpdatedByCustomer { get; set; }
}
