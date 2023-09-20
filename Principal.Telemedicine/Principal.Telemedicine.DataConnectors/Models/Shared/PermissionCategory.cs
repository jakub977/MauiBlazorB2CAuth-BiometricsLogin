using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Principal.Telemedicine.DataConnectors.Models.Shared;

/// <summary>
/// Table of categories of permissions
/// </summary>
[Table("PermissionCategory")]
public partial class PermissionCategory
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

    /// <summary>
    /// Link to dbo.Customer as an user who creates a permission category
    /// </summary>
    [ForeignKey("CreatedByCustomerId")]
    [InverseProperty("PermissionCategoryCreatedByCustomers")]
    public virtual Customer CreatedByCustomer { get; set; } = null!;

    /// <summary>
    /// Inverse collection of Permissions with the same permission category
    /// </summary>
    [InverseProperty("PermissionCategory")]
    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();

    /// <summary>
    /// Link to dbo.Customer as an user who updates a permission category
    /// </summary>
    [ForeignKey("UpdatedByCustomerId")]
    [InverseProperty("PermissionCategoryUpdatedByCustomers")]
    public virtual Customer? UpdatedByCustomer { get; set; }
}
