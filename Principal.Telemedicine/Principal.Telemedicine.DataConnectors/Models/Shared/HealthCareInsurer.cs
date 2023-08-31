using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Principal.Telemedicine.DataConnectors.Models.Shared;

/// <summary>
/// Table of health care insurers
/// </summary>
[Table("HealthCareInsurer")]
public partial class HealthCareInsurer
{
    /// <summary>
    /// Primary identifier of an insurer
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Bit identifier if an insurer is active
    /// </summary>
    [Required]
    public bool? Active { get; set; }

    /// <summary>
    /// Bit identifier if an insurer is deleted
    /// </summary>
    public bool Deleted { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who creates an insurer
    /// </summary>
    public int CreatedByCustomerId { get; set; }

    /// <summary>
    /// Date of insurance company creation, using coordinated universal time
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime CreatedDateUtc { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who updates an insurer
    /// </summary>
    public int? UpdatedByCustomerId { get; set; }

    /// <summary>
    /// Date of insurance company update, using coordinated universal time
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime? UpdateDateUtc { get; set; }

    /// <summary>
    /// Name of an insurer
    /// </summary>
    [StringLength(200)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Short name of an insurer
    /// </summary>
    [StringLength(10)]
    public string? ShortName { get; set; }

    /// <summary>
    /// Code of an insurer
    /// </summary>
    [StringLength(20)]
    [Unicode(false)]
    public string Code { get; set; } = null!;

    /// <summary>
    /// Link to dbo.Country as a country of insurer
    /// </summary>
    public int CountryId { get; set; }

    [ForeignKey("CreatedByCustomerId")]
    [InverseProperty("HealthCareInsurerCreatedByCustomers")]
    public virtual Customer CreatedByCustomer { get; set; } = null!;

    [InverseProperty("HealthCareInsurer")]
    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    [ForeignKey("UpdatedByCustomerId")]
    [InverseProperty("HealthCareInsurerUpdatedByCustomers")]
    public virtual Customer? UpdatedByCustomer { get; set; }
}
