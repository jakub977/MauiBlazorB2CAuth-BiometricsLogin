using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Principal.Telemedicine.DataConnectors.Models.Shared;

/// <summary>
/// Table of permissions of groups
/// </summary>
[Table("GroupPermission")]
public partial class GroupPermission
{
    /// <summary>
    /// Primary identifier of a group permission
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Bit identifier if a group permission is active
    /// </summary>
    [Required]
    public bool? Active { get; set; }

    /// <summary>
    /// Bit identifier if a group permission is deleted
    /// </summary>
    public bool Deleted { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who creates a group permission
    /// </summary>
    public int CreatedByCustomerId { get; set; }

    /// <summary>
    /// Date of group permission creation, using coordinated universal time
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime CreatedDateUtc { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who updates a group permission
    /// </summary>
    public int? UpdatedByCustomerId { get; set; }

    /// <summary>
    /// Date of group permission update, using coordinated universal time
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime? UpdateDateUtc { get; set; }

    /// <summary>
    /// Link to dbo.Group as a group to which permission is assigned
    /// </summary>
    public int GroupId { get; set; }

    /// <summary>
    /// Link to dbo.Permission as a permission which is assigned to group
    /// </summary>
    public int PermissionId { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who creates a group permission
    /// </summary>
    [ForeignKey("CreatedByCustomerId")]
    [InverseProperty("GroupPermissionCreatedByCustomers")]
    public virtual Customer CreatedByCustomer { get; set; } = null!;

    /// <summary>
    /// Link to dbo.Group as a group to which permission is assigned
    /// </summary>
    [ForeignKey("GroupId")]
    [InverseProperty("GroupPermissions")]
    public virtual Group Group { get; set; } = null!;

    /// <summary>
    /// Link to dbo.Permission as a permission which is assigned to group
    /// </summary>
    [ForeignKey("PermissionId")]
    [InverseProperty("GroupPermissions")]
    public virtual Permission Permission { get; set; } = null!;

    /// <summary>
    /// Link to dbo.Customer as an user who updates a group permission
    /// </summary>
    [ForeignKey("UpdatedByCustomerId")]
    [InverseProperty("GroupPermissionUpdatedByCustomers")]
    public virtual Customer? UpdatedByCustomer { get; set; }
}
