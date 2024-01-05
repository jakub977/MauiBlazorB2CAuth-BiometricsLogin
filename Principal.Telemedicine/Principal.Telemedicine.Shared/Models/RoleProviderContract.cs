using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Principal.Telemedicine.Shared.Models;

/// <summary>
/// Slouží pro správu Poskytovatelů
/// </summary>
[DataContract]
public class RoleProviderContract
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

    public string? lsrName { get; set; }

    /// <summary>
    /// Link to dbo.Role as a parent role, i.e. reference to original role
    /// </summary>
    public int? ParentRoleId { get; set; }

    public RoleProviderContract? ParentRoleContract { get; set; }

    /// <summary>
    /// Link to dbo.RoleCategoryCombination as a combination of role category and its subcategory
    /// </summary>
    public int RoleCategoryCombinationId { get; set; }

    /// <summary>
    /// Link to dbo.RoleCategoryCombination as a combination of role category and its subcategory
    /// </summary>
    [JsonPropertyName("roleCategoryCombinationObject")]
    public virtual RoleCategoryCombinationContract? RoleCategoryCombination { get; set; }
}
