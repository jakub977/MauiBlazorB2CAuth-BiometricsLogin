using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Principal.Telemedicine.Shared.Models;

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

    /// <summary>
    /// Link to dbo.Customer as an user who creates a permission
    /// </summary>
    [ForeignKey("CreatedByCustomerId")]
    [InverseProperty("PermissionCreatedByCustomers")]
    public virtual Customer CreatedByCustomer { get; set; } = null!;

    /// <summary>
    /// Inverse collection of GroupPermission as a permission which is assigned to group.
    /// </summary>
    [InverseProperty("Permission")]
    public virtual ICollection<GroupPermission> GroupPermissions { get; set; } = new List<GroupPermission>();

    /// <summary>
    /// Inverse collection of Permission as a inverse parent permission, used for hierarchy of permissions
    /// </summary>
    [InverseProperty("ParentPermission")]
    public virtual ICollection<Permission> InverseParentPermission { get; set; } = new List<Permission>();

    /// <summary>
    /// Link to dbo.Permission as a parent permission, used for hierarchy of permissions
    /// </summary>
    [ForeignKey("ParentPermissionId")]
    [InverseProperty("InverseParentPermission")]
    public virtual Permission? ParentPermission { get; set; }

    /// <summary>
    /// Link to dbo.PermissionCategory as a category of permission
    /// </summary>
    [ForeignKey("PermissionCategoryId")]
    [InverseProperty("Permissions")]
    public virtual PermissionCategory? PermissionCategory { get; set; }

    /// <summary>
    /// Link to dbo.PermissionType as a type of permission
    /// </summary>
    [ForeignKey("PermissionTypeId")]
    [InverseProperty("Permissions")]
    public virtual PermissionType PermissionType { get; set; } = null!;

    /// <summary>
    /// Inverse collection of RolePermissions
    /// </summary>
    [InverseProperty("Permission")]
    public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();

    /// <summary>
    /// Link to dbo.Subject as a subject of the application
    /// </summary>
    [ForeignKey("SubjectId")]
    [InverseProperty("Permissions")]
    public virtual Subject Subject { get; set; } = null!;

    /// <summary>
    /// Link to dbo.Customer as an user who updates a permission
    /// </summary>
    [ForeignKey("UpdatedByCustomerId")]
    [InverseProperty("PermissionUpdatedByCustomers")]
    public virtual Customer? UpdatedByCustomer { get; set; }

    /// <summary>
    /// InverseCollection of UserPermissions as a permission which is granted or denied to user
    /// </summary>
    [InverseProperty("Permission")]
    public virtual ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();

    /// <summary>
    /// Vrátí PermissionContract z Permission
    /// </summary>
    /// <param name="withSubject">Příznak, zda chceme vrátit i Subject (default TRUE)</param>
    /// <returns>PermissionContract</returns>
    public PermissionContract ConvertToPermissionContract(bool withSubject = true)
    {
        PermissionContract data = new PermissionContract();

        data.Active = Active;
        data.CreatedByCustomerId = CreatedByCustomerId;
        data.CreatedDateUtc = CreatedDateUtc;
        data.Deleted = Deleted;
        data.Description = Description;
        data.Id = Id;
        data.Name = Name;
        data.ParentPermissionId = ParentPermissionId;

        if (ParentPermission != null)
            data.ParentPermission = ParentPermission.ConvertToPermissionContract(withSubject);

        data.PermissionCategoryId = PermissionCategoryId;
        data.PermissionTypeId = PermissionTypeId;
        data.SubjectId = SubjectId;

        if (withSubject && Subject != null)
            data.Subject = Subject.ConvertToSubjectContract();

        data.SystemName = SystemName;
        data.UpdateDateUtc = UpdateDateUtc;
        data.UpdatedByCustomerId = UpdatedByCustomerId;

        return data;
    }
}
