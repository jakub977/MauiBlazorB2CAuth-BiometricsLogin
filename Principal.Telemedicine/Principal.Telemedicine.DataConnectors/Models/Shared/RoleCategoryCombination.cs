using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Principal.Telemedicine.DataConnectors.Models.Shared;

/// <summary>
/// Table of combinations of categories and subcategories of roles
/// </summary>
[Table("RoleCategoryCombination")]
public partial class RoleCategoryCombination
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
    public virtual Customer CreatedByCustomer { get; set; } = null!;

    [InverseProperty("RoleCategoryCombination")]
    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();

    [ForeignKey("UpdatedByCustomerId")]
    [InverseProperty("RoleCategoryCombinationUpdatedByCustomers")]
    public virtual Customer? UpdatedByCustomer { get; set; }
}
