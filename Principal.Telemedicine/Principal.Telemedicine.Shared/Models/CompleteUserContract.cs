using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Principal.Telemedicine.Shared.Models;

/// <summary>
/// User data contract derived from many cs.
/// </summary>
[DataContract]
public class CompleteUserContract
{
    private string _friendlyName;

    /// <summary>
    /// Primary identifier of an user
    /// </summary>
    [DataMember]
    public int Id { get; set; }

    /// <summary>
    /// Bit identifier if an user is active
    /// </summary>
    public bool Active { get; set; }

    /// <summary>
    /// Bit identifier if an user is deleted
    /// </summary>
    public bool Deleted { get; set; }

    /// <summary>
    /// Link to dbo.Provider as a provider which creates an user and where user is registered
    /// </summary>
    [DataMember]
    public int? CreatedByProviderId { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who creates an user
    /// </summary>
    [DataMember]
    public int? CreatedByCustomerId { get; set; }

    /// <summary>
    /// Date of user creation, using coordinated universal time
    /// </summary>
    [DataMember]
    public DateTime CreatedDateUtc { get; set; }

    /// <summary>
    /// Link to dbo.Customer as an user who updates an user
    /// </summary>
    [DataMember]
    public int? UpdatedByCustomerId { get; set; }

    /// <summary>
    /// Date of user update, using coordinated universal time
    /// </summary>
    [DataMember]
    public DateTime? UpdateDateUtc { get; set; }

    /// <summary>
    /// Link to dbo.Organization as an organization in which an user is registered
    /// </summary>
    [DataMember]
    public int? OrganizationId { get; set; }

    /// <summary>
    /// First name of an user
    /// </summary>
    [DataMember]
    public string FirstName { get; set; } = null!;

    /// <summary>
    /// Last name of an user
    /// </summary>
    [DataMember]
    public string LastName { get; set; } = null!;

    /// <summary>
    /// Link to dbo.GenderType as a gender type of an user
    /// </summary>
    [DataMember]
    public int? GenderTypeId { get; set; }

    /// <summary>
    /// Address line of an user (street, land registry number or house number, city)
    /// </summary>
    [DataMember]
    public string? AddressLine { get; set; }

    /// <summary>
    /// Postal code
    /// </summary>
    [DataMember]
    public string? PostalCode { get; set; }

    /// <summary>
    /// User's e-mail address
    /// </summary>
    [DataMember]
    public string Email { get; set; } = null!;

    /// <summary>
    /// Telephone number of an user
    /// </summary>
    [DataMember]
    public string? TelephoneNumber { get; set; }

    /// <summary>
    /// Personal identification number of an user
    /// </summary>
    [DataMember]
    public string? PersonalIdentificationNumber { get; set; }

    /// <summary>
    /// Birthdate of an user
    /// </summary>
    [DataMember]
    public DateTime? Birthdate { get; set; }

    /// <summary>
    /// Code of health care insurer of an user
    /// </summary>
    [DataMember]
    public string? HealthCareInsurerCode { get; set; }

    /// <summary>
    /// Public identifier of an user
    /// </summary>
    [DataMember]
    public string? PublicIdentifier { get; set; }

    /// <summary>
    /// Link to dbo.Picture as a photo of an user
    /// </summary>
    [DataMember]
    public int? PictureId { get; set; }

    /// <summary>
    /// Title before user's name
    /// </summary>
    [DataMember]
    public string? TitleBefore { get; set; }

    /// <summary>
    /// Title after user's name
    /// </summary>
    [DataMember]
    public string? TitleAfter { get; set; }

    /// <summary>
    /// Bit identifier if an account is the system account
    /// </summary>
    [DataMember]
    public bool IsSystemAccount { get; set; }

    /// <summary>
    /// Bit identifier if an account is the super admin account
    /// </summary>
    [DataMember]
    public bool IsSuperAdminAccount { get; set; }

    /// <summary>
    /// Bit identifier if an account is the organization admin account
    /// </summary>
    [DataMember]
    public bool IsOrganizationAdminAccount { get; set; }

    /// <summary>
    /// Bit identifier if an account is the provider admin account
    /// </summary>
    [DataMember]
    public bool IsProviderAdminAccount { get; set; }

    /// <summary>
    /// Comment of a super admin
    /// </summary>
    public string? AdminComment { get; set; }

    /// <summary>
    /// Password related to the user account
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// Password format identifier
    /// </summary>
    [JsonIgnore]
    public int PasswordFormatTypeId { get; set; }

    /// <summary>
    /// Salt of password
    /// </summary>
    public string? PasswordSalt { get; set; }

    /// <summary>
    /// Last IP address related to an user
    /// </summary>
    public string? LastIpAddress { get; set; }

    /// <summary>
    /// Last date of login, using coordinated universal time
    /// </summary>
    public DateTime? LastLoginDateUtc { get; set; }

    /// <summary>
    /// Last date of activity, using coordinated universal time
    /// </summary>
    public DateTime? LastActivityDateUtc { get; set; }

    /// <summary>
    /// Number of failed login attempts
    /// </summary>
    [JsonIgnore]
    public int InvalidLoginsCount { get; set; }

    /// <summary>
    /// Login token issued when login to API
    /// </summary>
    public string? ApiloginToken { get; set; }

    /// <summary>
    /// Last date of login to API, using coordinated universal time
    /// </summary>
    public DateTime? LastApiloginDateTime { get; set; }

    /// <summary>
    /// Bit identifier if login to API is enabled
    /// </summary>
    public bool? ApiloginEnabled { get; set; }

    /// <summary>
    /// Friendly name, i.e. full name of an user
    /// </summary>
    
    public string FriendlyName
    {
        get
        {
            if (string.IsNullOrEmpty(_friendlyName))
            {
                _friendlyName = FirstName;
                if (string.IsNullOrEmpty(_friendlyName))
                    _friendlyName = LastName;
                else
                {
                    if (!string.IsNullOrEmpty(LastName))
                    {
                        _friendlyName += " " + LastName;
                    }
                }
            }

            return _friendlyName;
        }
        set
        { _friendlyName = value; }
    }

    /// <summary>
    /// Global identifier of user, used for synchronization between dedicated DBs and central Azure DB
    /// </summary>
    [DataMember]
    public string GlobalId { get; set; } = null!;

    /// <summary>
    /// Link to dbo.ProfessionType as a profession of user
    /// </summary>
    public int? ProfessionTypeId { get; set; }

    public string? ProfessionName { get; set; }

    /// <summary>
    /// Employer of user
    /// </summary>
    [DataMember]
    public string? EmployerName { get; set; }

    /// <summary>
    /// Note to an user
    /// </summary>
    public string? Note { get; set; }

    /// <summary>
    /// Second telephone number of an user
    /// </summary>
    [DataMember]
    public string? TelephoneNumber2 { get; set; }

    /// <summary>
    /// Link to dbo.HealthCareInsurer as a health care insurer of an user
    /// </summary>
    [DataMember]
    public int? HealthCareInsurerId { get; set; }

    /// <summary>
    /// Birth identification number of an user
    /// </summary>
    [DataMember]
    public string? BirthIdentificationNumber { get; set; }

    /// <summary>
    /// User's home address
    /// </summary>
    [DataMember]
    public string? Street { get; set; }

    /// <summary>
    /// Link to dbo.City
    /// </summary>
    public int? CityId { get; set; }

    /// <summary>
    /// Bit identifier whether an user is at risk of any health issue
    /// </summary>
    public bool? IsRiskPatient { get; set; }

    [DataMember]
    [JsonPropertyName("cityObject")]
    public virtual AddressCityContract? City { get; set; }

    [DataMember]
    [JsonPropertyName("pictureObject")]
    public virtual PictureContract? Picture { get; set; }

    [DataMember]
    [JsonPropertyName("efUsers")]
    public ICollection<EffectiveUserContract> EffectiveUserUsers { get; set; } = new List<EffectiveUserContract>();

    [DataMember]
    public virtual GenderTypeContract? GenderType { get; set; }

    [DataMember]
    [JsonPropertyName("organizationObject")]
    public virtual OrganizationContract? Organization { get; set; }

    [DataMember]
    [JsonPropertyName("customerRoles")]
    public virtual ICollection<RoleMemberContract> RoleMemberDirectUsers { get; set; } = new List<RoleMemberContract>();

    [DataMember]
    [JsonPropertyName("userPermissions")]
    public virtual ICollection<UserPermissionContract> UserPermissionUsers { get; set; } = new List<UserPermissionContract>();

    /// <summary>
    /// Comment of a Gynecologist
    /// </summary>
    public string? GynecologistNote { get; set; }

    /// <summary>
    /// Language Id
    /// </summary>
    public int LanguageId { get; set; }

}
