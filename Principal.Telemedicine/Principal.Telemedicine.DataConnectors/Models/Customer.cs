using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Principal.Telemedicine.DataConnectors.Models;

/// <summary>
/// Table of users
/// </summary>
[Table("Customer")]
[Index("GlobalId", Name = "IX_Customer_RI_TEST", IsUnique = true)]
public class Customer
{
    /// <summary>
    /// Primary identifier of an user
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Bit identifier if an user is active
    /// </summary>
    [Required]
    public bool? Active { get; set; }

    /// <summary>
    /// Bit identifier if an user is deleted
    /// </summary>
    public bool Deleted { get; set; }

    /// <summary>
    /// Link to dbo.Provider as a provider which creates an user and where user is registered
    /// </summary>
    public int? CreatedByProviderId { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who creates an user
    /// </summary>
    public int? CreatedByCustomerId { get; set; }

    /// <summary>
    /// Date of user creation, using coordinated universal time
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime CreatedDateUtc { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who updates an user
    /// </summary>
    public int? UpdatedByCustomerId { get; set; }

    /// <summary>
    /// Date of user update, using coordinated universal time
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime? UpdateDateUtc { get; set; }

    /// <summary>
    /// Link to dbo.Organization as an organization in which an user is registered
    /// </summary>
    public int? OrganizationId { get; set; }

    /// <summary>
    /// First name of an user
    /// </summary>
    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    /// <summary>
    /// Last name of an user
    /// </summary>
    [StringLength(50)]
    public string LastName { get; set; } = null!;

    /// <summary>
    /// Link to dbo.GenderType as a gender type of an user
    /// </summary>
    public int? GenderTypeId { get; set; }

    /// <summary>
    /// Address line of an user (street, land registry number or house number, city)
    /// </summary>
    [StringLength(200)]
    public string? AddressLine { get; set; }

    /// <summary>
    /// Postal code
    /// </summary>
    [StringLength(20)]
    [Unicode(false)]
    public string? PostalCode { get; set; }

    /// <summary>
    /// User&apos;s e-mail address
    /// </summary>
    [StringLength(100)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    /// <summary>
    /// Telephone number of an user
    /// </summary>
    [StringLength(20)]
    [Unicode(false)]
    public string? TelephoneNumber { get; set; }

    /// <summary>
    /// Personal identification number of an user
    /// </summary>
    [StringLength(20)]
    [Unicode(false)]
    public string? PersonalIdentificationNumber { get; set; }

    /// <summary>
    /// Birthdate of an user
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime? Birthdate { get; set; }

    /// <summary>
    /// Code of health care insurer of an user
    /// </summary>
    [StringLength(20)]
    [Unicode(false)]
    public string? HealthCareInsurerCode { get; set; }

    /// <summary>
    /// Public identifier of an user
    /// </summary>
    [StringLength(50)]
    [Unicode(false)]
    public string? PublicIdentifier { get; set; }

    /// <summary>
    /// Link to dbo.Picture as a photo of an user
    /// </summary>
    public int? PictureId { get; set; }

    /// <summary>
    /// Title before user&apos;s name
    /// </summary>
    [StringLength(20)]
    [Unicode(false)]
    public string? TitleBefore { get; set; }

    /// <summary>
    /// Title after user&apos;s name
    /// </summary>
    [StringLength(20)]
    [Unicode(false)]
    public string? TitleAfter { get; set; }

    /// <summary>
    /// Bit identifier if an account is the system account
    /// </summary>
    public bool IsSystemAccount { get; set; }

    /// <summary>
    /// Bit identifier if an account is the super admin account
    /// </summary>
    public bool IsSuperAdminAccount { get; set; }

    /// <summary>
    /// Bit identifier if an account is the organization admin account
    /// </summary>
    public bool IsOrganizationAdminAccount { get; set; }

    /// <summary>
    /// Bit identifier if an account is the provider admin account
    /// </summary>
    public bool IsProviderAdminAccount { get; set; }

    /// <summary>
    /// Comment of a super admin
    /// </summary>
    [Unicode(false)]
    public string? AdminComment { get; set; }

    /// <summary>
    /// Password related to the user account
    /// </summary>
    [StringLength(500)]
    public string? Password { get; set; }

    /// <summary>
    /// Password format identifier
    /// </summary>
    public int PasswordFormatTypeId { get; set; }

    /// <summary>
    /// Salt of password
    /// </summary>
    [StringLength(500)]
    public string? PasswordSalt { get; set; }

    /// <summary>
    /// Last IP address related to an user
    /// </summary>
    [StringLength(100)]
    [Unicode(false)]
    public string? LastIpAddress { get; set; }

    /// <summary>
    /// Last date of login, using coordinated universal time
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime? LastLoginDateUtc { get; set; }

    /// <summary>
    /// Last date of activity, using coordinated universal time
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime? LastActivityDateUtc { get; set; }

    /// <summary>
    /// Number of failed login attempts
    /// </summary>
    public int InvalidLoginsCount { get; set; }

    /// <summary>
    /// Login token issued when login to API
    /// </summary>
    [Column("APILoginToken")]
    [StringLength(200)]
    [Unicode(false)]
    public string? ApiloginToken { get; set; }

    /// <summary>
    /// Last date of login to API, using coordinated universal time
    /// </summary>
    [Column("LastAPILoginDateTime", TypeName = "datetime")]
    public DateTime? LastApiloginDateTime { get; set; }

    /// <summary>
    /// Bit identifier if login to API is enabled
    /// </summary>
    [Required]
    [Column("APILoginEnabled")]
    public bool? ApiloginEnabled { get; set; }

    /// <summary>
    /// Friendly name, i.e. full name of an user
    /// </summary>
    [StringLength(101)]
    public string FriendlyName { get; set; } = null!;

    /// <summary>
    /// Global identifier of user, used for synchronization between dedicated DBs and central Azure DB
    /// </summary>
    [StringLength(100)]
    [Unicode(false)]
    public string GlobalId { get; set; } = null!;

    /// <summary>
    /// Link to dbo.ProfessionType as a profession of user
    /// </summary>
    public int? ProfessionTypeId { get; set; }

    [StringLength(200)]
    public string? ProfessionName { get; set; }

    /// <summary>
    /// Employer of user
    /// </summary>
    [StringLength(500)]
    public string? EmployerName { get; set; }

    /// <summary>
    /// Note to an user
    /// </summary>
    [StringLength(500)]
    public string? Note { get; set; }

    /// <summary>
    /// Second telephone number of an user
    /// </summary>
    [StringLength(20)]
    [Unicode(false)]
    public string? TelephoneNumber2 { get; set; }

    /// <summary>
    /// Link to dbo.HealthCareInsurer as a health care insurer of an user
    /// </summary>
    public int? HealthCareInsurerId { get; set; }

    /// <summary>
    /// Birth identification number of an user
    /// </summary>
    [StringLength(20)]
    [Unicode(false)]
    public string? BirthIdentificationNumber { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Street { get; set; }

    public int? CityId { get; set; }

    public bool? IsRiskPatient { get; set; }

    [ForeignKey("CreatedByCustomerId")]
    [InverseProperty("InverseCreatedByCustomer")]
    public virtual Customer? CreatedByCustomer { get; set; }

    [InverseProperty("CreatedByCustomer")]
    public virtual ICollection<Customer> InverseCreatedByCustomer { get; set; } = new List<Customer>();

    [InverseProperty("UpdatedByCustomer")]
    public virtual ICollection<Customer> InverseUpdatedByCustomer { get; set; } = new List<Customer>();

    [ForeignKey("UpdatedByCustomerId")]
    [InverseProperty("InverseUpdatedByCustomer")]
    public virtual Customer? UpdatedByCustomer { get; set; }
}
