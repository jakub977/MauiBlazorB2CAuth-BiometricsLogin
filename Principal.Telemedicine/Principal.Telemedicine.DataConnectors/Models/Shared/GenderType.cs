using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Principal.Telemedicine.DataConnectors.Models.Shared;

/// <summary>
/// System lookup table of types of gender
/// </summary>
[Table("GenderType")]
public partial class GenderType
{
    /// <summary>
    /// Primary identifier of a gender type
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Bit identifier if a gender type is active
    /// </summary>
    [Required]
    public bool? Active { get; set; }

    /// <summary>
    /// Bit identifier if a gender type is deleted
    /// </summary>
    public bool Deleted { get; set; }

    /// <summary>
    /// Date of gender type creation, using coordinated universal time
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime CreatedDateUtc { get; set; }

    /// <summary>
    /// Nname of a gender type
    /// </summary>
    [StringLength(200)]
    public string Name { get; set; } = null!;

    [InverseProperty("GenderType")]
    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
}
