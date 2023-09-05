using System.Runtime.Serialization;

namespace Principal.Telemedicine.Shared.Models;

/// <summary>
/// Data contract derived from SubjectType.cs
/// </summary>
[DataContract]
public class SubjectTypeContract
{
    /// <summary>
    /// Primary identifier of a subject type
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Bit identifier if a subject type is active
    /// </summary>
    public bool? Active { get; set; }

    /// <summary>
    /// Bit identifier if a subject type is deleted
    /// </summary>
    public bool Deleted { get; set; }

    /// <summary>
    /// Date of subject type creation, using coordinated universal time
    /// </summary>
    public DateTime CreatedDateUtc { get; set; }

    /// <summary>
    /// Name of a subject type
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Detailed description of a subject type
    /// </summary>
    public string? Description { get; set; }

    //public virtual ICollection<SubjectContract> Subjects { get; set; } = new List<SubjectContract>();
}
