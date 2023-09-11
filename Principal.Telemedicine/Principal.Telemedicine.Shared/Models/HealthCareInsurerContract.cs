namespace Principal.Telemedicine.Shared.Models;

/// <summary>
/// Data contract derived from HealthCareInsurer.cs
/// </summary>
public class HealthCareInsurerContract
{
    /// <summary>
    /// Primary identifier of an insurer
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Bit identifier if an insurer is active
    /// </summary>
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
    public DateTime CreatedDateUtc { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who updates an insurer
    /// </summary>
    public int? UpdatedByCustomerId { get; set; }

    /// <summary>
    /// Date of insurance company update, using coordinated universal time
    /// </summary>
    public DateTime? UpdateDateUtc { get; set; }

    /// <summary>
    /// Name of an insurer
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Short name of an insurer
    /// </summary>
    public string? ShortName { get; set; }

    /// <summary>
    /// Code of an insurer
    /// </summary>
    public string Code { get; set; } = null!;

    /// <summary>
    /// Link to dbo.Country as a country of insurer
    /// </summary>
    public int CountryId { get; set; }

    public virtual UserContract CreatedByCustomer { get; set; } = null!;

    public virtual ICollection<UserContract> Customers { get; set; } = new List<UserContract>();

    public virtual UserContract? UpdatedByCustomer { get; set; }
}
