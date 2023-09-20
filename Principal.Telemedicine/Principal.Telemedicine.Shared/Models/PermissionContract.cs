using System.Runtime.Serialization;
using System.Text.Json.Serialization;

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
    public int Id { get; set; }

    /// <summary>
    /// Bit identifier if a permission is active
    /// </summary>
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
    public DateTime CreatedDateUtc { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who updates a permission
    /// </summary>
    public int? UpdatedByCustomerId { get; set; }

    /// <summary>
    /// Date of permission update, using coordinated universal time
    /// </summary>
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
    public string Name { get; set; } = null!;

    /// <summary>
    /// System name of a permission
    /// </summary>
    public string SystemName { get; set; } = null!;

    /// <summary>
    /// Detailed description of a permission
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Link to dbo.Permission as a parent permission, used for hierarchy of permissions
    /// </summary>
    [JsonPropertyName("subjectObject.ParentSubjectObject")]
    public virtual PermissionContract? ParentPermission { get; set; }

    /// <summary>
    /// Link to dbo.Subject as a subject of the application
    /// </summary>
    [JsonPropertyName("subjectObject")]
    public virtual SubjectContract Subject { get; set; } = null!;

}
