namespace Principal.Telemedicine.Shared.Models;

/// <summary>
/// Data contract derived from ProfessionType.cs
/// </summary>
public class ProfessionTypeContract
{
    /// <summary>
    /// Primary identifier of a profession type
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Bit identifier if a profession type is active
    /// </summary>
    public bool? Active { get; set; }

    /// <summary>
    /// Bit identifier if a profession type is deleted
    /// </summary>
    public bool Deleted { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who creates a profession type
    /// </summary>
    public int CreatedByCustomerId { get; set; }

    /// <summary>
    /// Date of profession type creation, using coordinated universal time
    /// </summary>
    public DateTime CreatedDateUtc { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who updates a profession type
    /// </summary>
    public int? UpdatedByCustomerId { get; set; }

    /// <summary>
    /// Date of profession type update, using coordinated universal time
    /// </summary>
    public DateTime? UpdateDateUtc { get; set; }

    /// <summary>
    /// Name of a profession type
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Detailed description of a profession type
    /// </summary>
    public string? Description { get; set; }


    //public virtual UserContract CreatedByCustomer { get; set; } = null!;

    //public virtual ICollection<UserContract> Customers { get; set; } = new List<UserContract>();

    //public virtual UserContract? UpdatedByCustomer { get; set; }
}
