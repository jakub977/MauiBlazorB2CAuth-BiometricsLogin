using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Principal.Telemedicine.DataConnectors.Models.Shared;

/// <summary>
/// System lookup table of permission types (primarly CRUD)
/// </summary>
[Table("PermissionType")]
public partial class PermissionType
{
    /// <summary>
    /// Primary identifier of a permission type
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Bit identifier if a permission type is active
    /// </summary>
    [Required]
    public bool? Active { get; set; }

    /// <summary>
    /// Bit identifier if a permission type is deleted
    /// </summary>
    public bool Deleted { get; set; }

    /// <summary>
    /// Date of permission type creation, using coordinated universal time
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime CreatedDateUtc { get; set; }

    /// <summary>
    /// Name of a permission type
    /// </summary>
    [StringLength(200)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Detailed description of a permission type
    /// </summary>
    [StringLength(200)]
    public string? Description { get; set; }

    /// <summary>
    /// Inverse collection of Permissions with specific PermissionType
    /// </summary>
    [InverseProperty("PermissionType")]
    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();
}
