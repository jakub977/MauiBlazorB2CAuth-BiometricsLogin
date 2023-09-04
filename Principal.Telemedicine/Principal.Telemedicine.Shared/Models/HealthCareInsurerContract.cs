using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Principal.Telemedicine.Shared.Models;

public class HealthCareInsurerContract
{
    /// <summary>
    /// Primary identifier of an insurer
    /// </summary>
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
    public string Code { get; set; } = null!;

    /// <summary>
    /// Link to dbo.Country as a country of insurer
    /// </summary>
    public int CountryId { get; set; }

    [ForeignKey("CreatedByCustomerId")]
    [InverseProperty("HealthCareInsurerCreatedByCustomers")]
    public virtual UserContract CreatedByCustomer { get; set; } = null!;

    [InverseProperty("HealthCareInsurer")]
    public virtual ICollection<UserContract> Customers { get; set; } = new List<UserContract>();

    [ForeignKey("UpdatedByCustomerId")]
    [InverseProperty("HealthCareInsurerUpdatedByCustomers")]
    public virtual UserContract? UpdatedByCustomer { get; set; }
}
