﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
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

    [ForeignKey("CreatedByCustomerId")]
    [InverseProperty("RoleSubCategoryCreatedByCustomers")]
    public virtual UserContract CreatedByCustomer { get; set; } = null!;

    [InverseProperty("RoleSubCategory")]
    public virtual ICollection<RoleCategoryCombinationContract> RoleCategoryCombinations { get; set; } = new List<RoleCategoryCombinationContract>();

    [ForeignKey("UpdatedByCustomerId")]
    [InverseProperty("RoleSubCategoryUpdatedByCustomers")]
    public virtual UserContract? UpdatedByCustomer { get; set; }
}
