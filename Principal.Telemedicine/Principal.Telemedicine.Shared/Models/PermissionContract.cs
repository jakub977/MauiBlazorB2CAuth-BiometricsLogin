using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Principal.Telemedicine.Shared.Models;

/// <summary>
/// Data contract derived from Permission.cs
/// </summary>
[DataContract]
public class PermissionContract
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
    public string SystemName { get; set; } = null!;

    /// <summary>
    /// Detailed description of a permission
    /// </summary>
    [StringLength(200)]
    public string? Description { get; set; }

    [ForeignKey("CreatedByCustomerId")]
    [InverseProperty("PermissionCreatedByCustomers")]
    public virtual UserContract CreatedByCustomer { get; set; } = null!;

    [InverseProperty("Permission")]
    public virtual ICollection<GroupPermissionContract> GroupPermissions { get; set; } = new List<GroupPermissionContract>();

    [InverseProperty("ParentPermission")]
    public virtual ICollection<PermissionContract> InverseParentPermission { get; set; } = new List<PermissionContract>();

    [ForeignKey("ParentPermissionId")]
    [InverseProperty("InverseParentPermission")]
    public virtual PermissionContract? ParentPermission { get; set; }

    [ForeignKey("PermissionCategoryId")]
    [InverseProperty("Permissions")]
    public virtual PermissionCategoryContract? PermissionCategory { get; set; }

    [ForeignKey("PermissionTypeId")]
    [InverseProperty("Permissions")]
    public virtual PermissionTypeContract PermissionType { get; set; } = null!;

    [InverseProperty("Permission")]
    public virtual ICollection<RolePermissionContract> RolePermissions { get; set; } = new List<RolePermissionContract>();

    [ForeignKey("SubjectId")]
    [InverseProperty("Permissions")]
    public virtual SubjectContract Subject { get; set; } = null!;

    [ForeignKey("UpdatedByCustomerId")]
    [InverseProperty("PermissionUpdatedByCustomers")]
    public virtual UserContract? UpdatedByCustomer { get; set; }

    [InverseProperty("Permission")]
    public virtual ICollection<UserPermissionContract> UserPermissions { get; set; } = new List<UserPermissionContract>();
}
