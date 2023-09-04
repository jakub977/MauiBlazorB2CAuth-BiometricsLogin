using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Principal.Telemedicine.Shared.Models;

/// <summary>
/// Data contract derived from GroupPermission.cs
/// </summary>
[DataContract]
public class GroupPermissionContract
{
    /// <summary>
    /// Primary identifier of a group permission
    /// </summary>
    [DataMember]
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

    [ForeignKey("CreatedByCustomerId")]
    [InverseProperty("GroupPermissionCreatedByCustomers")]
    public virtual UserContract CreatedByCustomer { get; set; } = null!;

    [ForeignKey("GroupId")]
    [InverseProperty("GroupPermissions")]
    public virtual GroupContract Group { get; set; } = null!;

    [ForeignKey("PermissionId")]
    [InverseProperty("GroupPermissions")]
    public virtual PermissionContract Permission { get; set; } = null!;

    [ForeignKey("UpdatedByCustomerId")]
    [InverseProperty("GroupPermissionUpdatedByCustomers")]
    public virtual UserContract? UpdatedByCustomer { get; set; }
}
