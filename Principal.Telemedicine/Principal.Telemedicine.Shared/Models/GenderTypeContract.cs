namespace Principal.Telemedicine.Shared.Models;

/// <summary>
/// Data contract derived from GenderType.cs
/// </summary>
public class GenderTypeContract
{
    /// <summary>
    /// Primary identifier of a gender type
    /// </summary>

    public int Id { get; set; }

    /// <summary>
    /// Bit identifier if a gender type is active
    /// </summary>
    public bool? Active { get; set; }

    /// <summary>
    /// Bit identifier if a gender type is deleted
    /// </summary>
    public bool Deleted { get; set; }

    /// <summary>
    /// Date of gender type creation, using coordinated universal time
    /// </summary>
    public DateTime CreatedDateUtc { get; set; }

    /// <summary>
    /// Nname of a gender type
    /// </summary>
    public string Name { get; set; } = null!;

    //public virtual ICollection<UserContract> Customers { get; set; } = new List<UserContract>();
}
