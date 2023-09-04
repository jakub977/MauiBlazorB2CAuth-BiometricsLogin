using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Principal.Telemedicine.Shared.Models;

/// <summary>
/// Data contract derived from RoleCategoryCombination.cs
/// </summary>
[DataContract]
public class RoleCategoryCombinationContract
{
    /// <summary>
    /// Primary identifier of a combination
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Bit identifier if combination is active
    /// </summary>
    [Required]
    public bool? Active { get; set; }

    /// <summary>
    /// Bit identifier if combination is deleted
    /// </summary>
    public bool Deleted { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who creates combination
    /// </summary>
    public int CreatedByCustomerId { get; set; }

    /// <summary>
    /// Date of combination creation, using coordinated universal time
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime CreatedDateUtc { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who updates combination
    /// </summary>
    public int? UpdatedByCustomerId { get; set; }

    /// <summary>
    /// Date of combination update, using coordinated universal time
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime? UpdateDateUtc { get; set; }

    /// <summary>
    /// Link to dbo.RoleCategory as a role category of combination
    /// </summary>
    public int RoleCategoryId { get; set; }

    /// <summary>
    /// Link to dbo.RoleSubCategory as a role subcategory of combination
    /// </summary>
    public int? RoleSubCategoryId { get; set; }

    /// <summary>
    /// Name of a combination
    /// </summary>
    [StringLength(200)]
    public string? Name { get; set; }

    [ForeignKey("CreatedByCustomerId")]
    [InverseProperty("RoleCategoryCombinationCreatedByCustomers")]
    public virtual UserContract CreatedByCustomer { get; set; } = null!;

    [ForeignKey("RoleCategoryId")]
    [InverseProperty("RoleCategoryCombinations")]
    public virtual RoleCategoryContract RoleCategory { get; set; } = null!;

    [ForeignKey("RoleSubCategoryId")]
    [InverseProperty("RoleCategoryCombinations")]
    public virtual RoleSubCategoryContract? RoleSubCategory { get; set; }

    [InverseProperty("RoleCategoryCombination")]
    public virtual ICollection<RoleContract> Roles { get; set; } = new List<RoleContract>();

    [ForeignKey("UpdatedByCustomerId")]
    [InverseProperty("RoleCategoryCombinationUpdatedByCustomers")]
    public virtual UserContract? UpdatedByCustomer { get; set; }
}
