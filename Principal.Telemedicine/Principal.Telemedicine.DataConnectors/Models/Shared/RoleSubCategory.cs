﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Principal.Telemedicine.DataConnectors.Models.Shared;

/// <summary>
/// Table of subcategories of roles
/// </summary>
[Table("RoleSubCategory")]
public partial class RoleSubCategory
{
    /// <summary>
    /// Primary identifier of a subcategory
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Bit identifier if subcategory is active
    /// </summary>
    [Required]
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
    [Column(TypeName = "datetime")]
    public DateTime CreatedDateUtc { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who updates subcategory
    /// </summary>
    public int? UpdatedByCustomerId { get; set; }

    /// <summary>
    /// Date of subcategory update, using coordinated universal time
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime? UpdateDateUtc { get; set; }

    /// <summary>
    /// Name of a subcategory
    /// </summary>
    [StringLength(200)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Detailed description of a subcategory
    /// </summary>
    [StringLength(200)]
    public string? Description { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who creates a role sub category
    /// </summary>
    [ForeignKey("CreatedByCustomerId")]
    [InverseProperty("RoleSubCategoryCreatedByCustomers")]
    public virtual Customer CreatedByCustomer { get; set; } = null!;

    /// <summary>
    /// Inverse collection of RoleCategoryCombinations to which role sub category is related
    /// </summary>
    [InverseProperty("RoleSubCategory")]
    public virtual ICollection<RoleCategoryCombination> RoleCategoryCombinations { get; set; } = new List<RoleCategoryCombination>();

    /// <summary>
    /// Link to dbo.Customer as an user who updates a role sub category
    /// </summary>
    [ForeignKey("UpdatedByCustomerId")]
    [InverseProperty("RoleSubCategoryUpdatedByCustomers")]
    public virtual Customer? UpdatedByCustomer { get; set; }
}