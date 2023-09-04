using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Principal.Telemedicine.Shared.Models;

/// <summary>
/// Data contract derived from AddressCity.cs
/// </summary>
[DataContract]
public class AddressCityContract
{
    /// <summary>
    /// Primary identifier of city
    /// </summary>

    public int Id { get; set; }

    /// <summary>
    /// Bit identifier if city is active
    /// </summary>
    [Required]
    public bool? Active { get; set; }

    /// <summary>
    /// Bit identifier if city is deleted
    /// </summary>
    public bool Deleted { get; set; }

    /// <summary>
    /// Date of city creation, using coordinated universal time
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime CreatedDateUtc { get; set; }

    /// <summary>
    /// Date of city update, using coordinated universal time
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime? UpdateDateUtc { get; set; }

    /// <summary>
    /// City code
    /// </summary>
    [StringLength(10)]
    public string Code { get; set; } = null!;

    /// <summary>
    /// City name
    /// </summary>
    [StringLength(200)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Link to dbo.AddressRegion as a parent region of city
    /// </summary>
    public int IdAddressRegion { get; set; }

    /// <summary>
    /// Link to dbo.AddressDistrict as a parent district of city
    /// </summary>
    public int IdAddressDistrict { get; set; }

    /// <summary>
    /// Link to dbo.AddressMunicipalityWithExtendedCompetence as a parent municipality with extended competence of city
    /// </summary>
    public int IdAddressMunicipalityWithExtendedCompetence { get; set; }

    [StringLength(1000)]
    public string? ExtendedName { get; set; }

    [InverseProperty("City")]
    public virtual ICollection<UserContract> Customers { get; set; } = new List<UserContract>();

    [InverseProperty("City")]
    public virtual ICollection<ProviderContract> Providers { get; set; } = new List<ProviderContract>();
}
