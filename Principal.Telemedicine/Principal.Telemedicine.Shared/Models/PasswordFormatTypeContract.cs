using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Principal.Telemedicine.Shared.Models;

public class PasswordFormatTypeContract
{
    /// <summary>
    /// Primary identifier of a password format
    /// </summary>
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

    [InverseProperty("PasswordFormatType")]
    public virtual ICollection<UserContract> Customers { get; set; } = new List<UserContract>();
}
