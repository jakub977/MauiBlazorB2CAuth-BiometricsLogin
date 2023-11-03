using Principal.Telemedicine.Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    /// Name of a gender type
    /// </summary>
    [StringLength(200)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Inverse collection of Customers with the same selected GenderType
    /// </summary>
    [InverseProperty("GenderType")]
    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    /// <summary>
    /// Vrátí GenderTypeContract z GenderType
    /// </summary>
    /// <returns>GenderTypeContract</returns>
    public GenderTypeContract ConvertToGenderTypeContract()
    {
        GenderTypeContract data = new GenderTypeContract();

        data.Active = Active;
        data.CreatedDateUtc = CreatedDateUtc;
        data.Deleted = Deleted;
        data.Id = Id;
        data.Name = Name;

        return data;
    }
}
