using Principal.Telemedicine.Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Principal.Telemedicine.DataConnectors.Models.Shared;

/// <summary>
/// Table of adress city of customer
/// </summary>
[Table("AddressCity")]
public partial class AddressCity
{
    /// <summary>
    /// Primary identifier of city
    /// </summary>
    [Key]
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
    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    [InverseProperty("City")]
    public virtual ICollection<Provider> Providers { get; set; } = new List<Provider>();

    /// <summary>
    /// Vytvoří AddressCityContract z AddressCity
    /// </summary>
    /// <returns>AddressCityContract</returns>
    public AddressCityContract ConvertToAddressCityContract()
    {
        AddressCityContract data = new AddressCityContract();

        data.Active = Active;
        data.Code = Code;
        data.CreatedDateUtc = CreatedDateUtc;
        data.Deleted = Deleted;
        data.ExtendedName = ExtendedName;
        data.Id = Id;
        data.IdAddressDistrict = IdAddressDistrict;
        data.IdAddressMunicipalityWithExtendedCompetence = IdAddressMunicipalityWithExtendedCompetence;
        data.IdAddressRegion = IdAddressRegion;
        data.Name = Name;
        data.UpdateDateUtc = UpdateDateUtc;

        return data;
    }
}
