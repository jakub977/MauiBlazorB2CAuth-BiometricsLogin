using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Principal.Telemedicine.DataConnectors.Models.Shared;

/// <summary>
/// System lookup table of password formats
/// </summary>
[Table("PasswordFormatType")]
public partial class PasswordFormatType
{
    /// <summary>
    /// Primary identifier of a password format
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Bit identifier if a password format is active
    /// </summary>
    [Required]
    public bool? Active { get; set; }

    /// <summary>
    /// Bit identifier if a password format is deleted
    /// </summary>
    public bool Deleted { get; set; }

    /// <summary>
    /// Date of password format creation, using coordinated universal time
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime CreatedDateUtc { get; set; }

    /// <summary>
    /// Name of a password format
    /// </summary>
    [StringLength(200)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Inverse collection of Customers
    /// </summary>
    [InverseProperty("PasswordFormatType")]
    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
}
