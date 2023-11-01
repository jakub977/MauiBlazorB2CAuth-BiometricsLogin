using System.Runtime.Serialization;
using System.Text.Json.Serialization;

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
    public int Id { get; set; }

    /// <summary>
    /// Bit identifier if a group permission is active
    /// </summary>
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
    public DateTime CreatedDateUtc { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who updates a group permission
    /// </summary>
    public int? UpdatedByCustomerId { get; set; }

    /// <summary>
    /// Date of group permission update, using coordinated universal time
    /// </summary>
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
    public virtual UserContract CreatedByCustomer { get; set; } = null!;

    /// <summary>
    /// Link to dbo.Group as a group to which permission is assigned
    /// </summary>
    public virtual GroupContract Group { get; set; } = null!;

    /// <summary>
    /// Link to dbo.Permission as a permission which is assigned to group
    /// </summary>
    [JsonPropertyName("permissionObject")]
    public virtual PermissionContract Permission { get; set; } = null!;

    /// <summary>
    /// Link to dbo.Customer as an user who updates a group permission
    /// </summary>
    public virtual UserContract? UpdatedByCustomer { get; set; }
}
