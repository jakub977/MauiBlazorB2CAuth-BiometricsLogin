using System.Runtime.Serialization;
using System.Text.Json.Serialization;

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
    public int Id { get; set; }

    /// <summary>
    /// Bit identifier if role is active
    /// </summary>
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
    public DateTime CreatedDateUtc { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who updates role
    /// </summary>
    public int? UpdatedByCustomerId { get; set; }

    /// <summary>
    /// Date of role update, using coordinated universal time
    /// </summary>
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
    public string Name { get; set; } = null!;

    /// <summary>
    /// Detailed description of a role
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Link to dbo.Role as a parent role, i.e. reference to original role
    /// </summary>
    public int? ParentRoleId { get; set; }

    [JsonPropertyName("parentRoleObject")]
    public virtual RoleContract? ParentRole { get; set; }

    /// <summary>
    /// Link to dbo.RoleCategoryCombination as a combination of role category and its subcategory
    /// </summary>
    public int RoleCategoryCombinationId { get; set; }

    /// <summary>
    /// Link to dbo.Provider as a provider to whom role relates
    /// </summary>
    [JsonPropertyName("providerObject")]
    public virtual ProviderContract? Provider { get; set; }

    /// <summary>
    /// Link to dbo.RoleCategoryCombination as a combination of role category and its subcategory
    /// </summary>
    [JsonPropertyName("roleCategoryCombinationObject")]
    public virtual RoleCategoryCombinationContract? RoleCategoryCombination { get; set; }

    /// <summary>
    /// Inverse collection of RolePermissions to which is a role related
    /// </summary>
    public virtual ICollection<RolePermissionContract> RolePermissions { get; set; } = new List<RolePermissionContract>();
}
