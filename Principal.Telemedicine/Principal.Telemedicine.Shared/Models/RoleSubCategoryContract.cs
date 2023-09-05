using System.Runtime.Serialization;

namespace Principal.Telemedicine.Shared.Models;

/// <summary>
/// Data contract derived from RoleSubCategory.cs
/// </summary>
[DataContract]
public class RoleSubCategoryContract
{
    /// <summary>
    /// Primary identifier of a subcategory
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Bit identifier if subcategory is active
    /// </summary>
    public bool? Active { get; set; }

    /// <summary>
    /// Bit identifier if subcategory is deleted
    /// </summary>
    public bool Deleted { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who creates subcategory
    /// </summary>
    public int CreatedByCustomerId { get; set; }

    /// <summary>
    /// Date of subcategory creation, using coordinated universal time
    /// </summary>
    public DateTime CreatedDateUtc { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who updates subcategory
    /// </summary>
    public int? UpdatedByCustomerId { get; set; }

    /// <summary>
    /// Date of subcategory update, using coordinated universal time
    /// </summary>
    public DateTime? UpdateDateUtc { get; set; }

    /// <summary>
    /// Name of a subcategory
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Detailed description of a subcategory
    /// </summary>
    public string? Description { get; set; }

    //public virtual UserContract CreatedByCustomer { get; set; } = null!;

    //public virtual ICollection<RoleCategoryCombinationContract> RoleCategoryCombinations { get; set; } = new List<RoleCategoryCombinationContract>();

    //public virtual UserContract? UpdatedByCustomer { get; set; }
}
