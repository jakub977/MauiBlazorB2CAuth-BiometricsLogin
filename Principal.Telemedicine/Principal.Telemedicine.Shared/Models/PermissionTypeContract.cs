using System.Runtime.Serialization;

namespace Principal.Telemedicine.Shared.Models;

/// <summary>
/// Data contract derived from PermissionType.cs
/// </summary>
[DataContract]
public class PermissionTypeContract
{
    /// <summary>
    /// Primary identifier of a permission type
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Bit identifier if a permission type is active
    /// </summary>
    public bool? Active { get; set; }

    /// <summary>
    /// Bit identifier if a permission type is deleted
    /// </summary>
    public bool Deleted { get; set; }

    /// <summary>
    /// Date of permission type creation, using coordinated universal time
    /// </summary>
    public DateTime CreatedDateUtc { get; set; }

    /// <summary>
    /// Name of a permission type
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Detailed description of a permission type
    /// </summary>
    public string? Description { get; set; }
}
