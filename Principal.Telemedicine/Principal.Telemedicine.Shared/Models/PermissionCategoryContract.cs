using System.Runtime.Serialization;

namespace Principal.Telemedicine.Shared.Models;

/// <summary>
/// Data contract derived from PermissionCategory.cs
/// </summary>
[DataContract]
public class PermissionCategoryContract
{
    // <summary>
    /// Primary identifier of a category
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Bit identifier if category is active
    /// </summary>
    public bool? Active { get; set; }

    /// <summary>
    /// Bit identifier if category is deleted
    /// </summary>
    public bool Deleted { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who creates category
    /// </summary>
    public int CreatedByCustomerId { get; set; }

    /// <summary>
    /// Date of category creation, using coordinated universal time
    /// </summary>
    public DateTime CreatedDateUtc { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who updates category
    /// </summary>
    public int? UpdatedByCustomerId { get; set; }

    /// <summary>
    /// Date of category update, using coordinated universal time
    /// </summary>
    public DateTime? UpdateDateUtc { get; set; }

    /// <summary>
    /// Name of a category
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Detailed description of a category
    /// </summary>
    public string? Description { get; set; }


    //public virtual UserContract CreatedByCustomer { get; set; } = null!;

    //public virtual ICollection<PermissionContract> Permissions { get; set; } = new List<PermissionContract>();

    //public virtual UserContract? UpdatedByCustomer { get; set; }
}
