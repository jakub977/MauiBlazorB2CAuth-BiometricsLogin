using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Principal.Telemedicine.Shared.Models;

public class GenderTypeContract
{
    /// <summary>
    /// Primary identifier of a gender type
    /// </summary>

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
    public virtual ICollection<UserContract> Customers { get; set; } = new List<UserContract>();
}
