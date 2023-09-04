using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Principal.Telemedicine.Shared.Models;

/// <summary>
/// Data contract derived from RoleCategory.cs
/// </summary>
[DataContract]
public class RoleCategoryContract
{
    /// <summary>
    /// Primary identifier of a category
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Bit identifier if category is active
    /// </summary>
    [Required]
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
    [Column(TypeName = "datetime")]
    public DateTime CreatedDateUtc { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who updates category
    /// </summary>
    public int? UpdatedByCustomerId { get; set; }

    /// <summary>
    /// Date of category update, using coordinated universal time
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime? UpdateDateUtc { get; set; }

    /// <summary>
    /// Name of a category
    /// </summary>
    [StringLength(200)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Detailed description of a category
    /// </summary>
    [StringLength(200)]
    public string? Description { get; set; }

    [ForeignKey("CreatedByCustomerId")]
    [InverseProperty("RoleCategoryCreatedByCustomers")]
    public virtual UserContract CreatedByCustomer { get; set; } = null!;

    [InverseProperty("RoleCategory")]
    public virtual ICollection<RoleCategoryCombinationContract> RoleCategoryCombinations { get; set; } = new List<RoleCategoryCombinationContract>();

    [ForeignKey("UpdatedByCustomerId")]
    [InverseProperty("RoleCategoryUpdatedByCustomers")]
    public virtual UserContract? UpdatedByCustomer { get; set; }
}
