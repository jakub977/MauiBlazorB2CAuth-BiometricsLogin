using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Principal.Telemedicine.DataConnectors.Models.Shared;

/// <summary>
/// Lookup table of types of professions
/// </summary>
[Table("ProfessionType")]
public partial class ProfessionType
{
    /// <summary>
    /// Primary identifier of a profession type
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Bit identifier if a profession type is active
    /// </summary>
    [Required]
    public bool? Active { get; set; }

    /// <summary>
    /// Bit identifier if a profession type is deleted
    /// </summary>
    public bool Deleted { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who creates a profession type
    /// </summary>
    public int CreatedByCustomerId { get; set; }

    /// <summary>
    /// Date of profession type creation, using coordinated universal time
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime CreatedDateUtc { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who updates a profession type
    /// </summary>
    public int? UpdatedByCustomerId { get; set; }

    /// <summary>
    /// Date of profession type update, using coordinated universal time
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime? UpdateDateUtc { get; set; }

    /// <summary>
    /// Name of a profession type
    /// </summary>
    [StringLength(200)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Detailed description of a profession type
    /// </summary>
    [StringLength(200)]
    public string? Description { get; set; }

    [ForeignKey("CreatedByCustomerId")]
    [InverseProperty("ProfessionTypeCreatedByCustomers")]
    public virtual Customer CreatedByCustomer { get; set; } = null!;

    [InverseProperty("ProfessionType")]
    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    [ForeignKey("UpdatedByCustomerId")]
    [InverseProperty("ProfessionTypeUpdatedByCustomers")]
    public virtual Customer? UpdatedByCustomer { get; set; }
}
