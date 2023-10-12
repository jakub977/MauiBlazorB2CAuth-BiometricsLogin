using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Principal.Telemedicine.DataConnectors.Models.Shared;

/// <summary>
/// Table of users
/// </summary>
[Table("Customer")]
[Index("GlobalId", Name = "IX_Customer_RI_TEST", IsUnique = true)]
public partial class Customer
{
    /// <summary>
    /// Primary identifier of a user
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Bit identifier if a user is active
    /// </summary>
    [Required]
    public bool? Active { get; set; }

    /// <summary>
    /// Bit identifier if a user is deleted
    /// </summary>
    public bool Deleted { get; set; }

    /// <summary>
    /// Link to dbo.Provider as a provider which creates a user and where user is registered
    /// </summary>
    public int? CreatedByProviderId { get; set; }

    /// <summary>
    /// Link to dbo.Customer as a user who creates an user
    /// </summary>
    public int? CreatedByCustomerId { get; set; }

    /// <summary>
    /// Date of user creation, using coordinated universal time
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime CreatedDateUtc { get; set; }

    /// <summary>
    /// Link to dbo.Customer as a user who updates an user
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
    /// First name of a user
    /// </summary>
    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    /// <summary>
    /// Last name of a user
    /// </summary>
    [StringLength(50)]
    public string LastName { get; set; } = null!;

    /// <summary>
    /// Link to dbo.GenderType as a gender type of a user
    /// </summary>
    public int? GenderTypeId { get; set; }

    /// <summary>
    /// Address line of a user (street, land registry number or house number, city)
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
    /// Users e-mail address
    /// </summary>
    [StringLength(100)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    /// <summary>
    /// Telephone number of a user
    /// </summary>
    [StringLength(20)]
    [Unicode(false)]
    public string? TelephoneNumber { get; set; }

    /// <summary>
    /// Personal identification number of a user
    /// </summary>
    [StringLength(20)]
    [Unicode(false)]
    public string? PersonalIdentificationNumber { get; set; }

    /// <summary>
    /// Birthdate of a user
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime? Birthdate { get; set; }

    /// <summary>
    /// Code of health care insurer of a user
    /// </summary>
    [StringLength(20)]
    [Unicode(false)]
    public string? HealthCareInsurerCode { get; set; }

    /// <summary>
    /// Public identifier of a user
    /// </summary>
    [StringLength(50)]
    [Unicode(false)]
    public string? PublicIdentifier { get; set; }

    /// <summary>
    /// Link to dbo.Picture as a photo of a user
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
    /// Last IP address related to a user
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
    [JsonIgnore]
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

    /// <summary>
    /// Name of users profession
    /// </summary>
    [StringLength(200)]
    public string? ProfessionName { get; set; }

    /// <summary>
    /// Employer of user
    /// </summary>
    [StringLength(500)]
    public string? EmployerName { get; set; }

    /// <summary>
    /// Note to a user
    /// </summary>
    [StringLength(500)]
    public string? Note { get; set; }

    /// <summary>
    /// Second telephone number of a user
    /// </summary>
    [StringLength(20)]
    [Unicode(false)]
    public string? TelephoneNumber2 { get; set; }

    /// <summary>
    /// Link to dbo.HealthCareInsurer as a health care insurer of a user
    /// </summary>
    public int? HealthCareInsurerId { get; set; }

    /// <summary>
    /// Birth identification number of a user
    /// </summary>
    [StringLength(20)]
    [Unicode(false)]
    public string? BirthIdentificationNumber { get; set; }

    /// <summary>
    /// Street where user lives
    /// </summary>
    [StringLength(100)]
    [Unicode(false)]
    public string? Street { get; set; }

    /// <summary>
    /// Link to dbo.AddressCity as a address of user
    /// </summary>
    public int? CityId { get; set; }

    /// <summary>
    /// Bit identifier whether a user is at risk of any health issue
    /// </summary>
    public bool? IsRiskPatient { get; set; }

    /// <summary>
    /// Link to dbo.AddressCity as a city where user lives
    /// </summary>
    [ForeignKey("CityId")]
    [InverseProperty("Customers")]
    public virtual AddressCity? City { get; set; }

    /// <summary>
    /// Link to dbo.Customer as a user who creates an user
    /// </summary>
    [ForeignKey("CreatedByCustomerId")]
    [InverseProperty("InverseCreatedByCustomer")]
    public virtual Customer? CreatedByCustomer { get; set; }

    /// <summary>
    /// Link to dbo.Provider as a provider who creates a user
    /// </summary>
    [ForeignKey("CreatedByProviderId")]
    [InverseProperty("Customers")]
    public virtual Provider? CreatedByProvider { get; set; }

    /// <summary>
    /// Inverse collection of EffectiveUser created by user
    /// </summary>
    [InverseProperty("CreatedByCustomer")]
    public virtual ICollection<EffectiveUser> EffectiveUserCreatedByCustomers { get; set; } = new List<EffectiveUser>();

    /// <summary>
    /// Inverse collection of EffectiveUser updated by user
    /// </summary>
    [InverseProperty("UpdatedByCustomer")]
    public virtual ICollection<EffectiveUser> EffectiveUserUpdatedByCustomers { get; set; } = new List<EffectiveUser>();

    /// <summary>
    /// Inverse collection of users EffectiveUsers 
    /// </summary>
    [InverseProperty("User")]
    public virtual ICollection<EffectiveUser> EffectiveUserUsers { get; set; } = new List<EffectiveUser>();

    /// <summary>
    /// Link to dbo.GenderType as a users gender
    /// </summary>
    [ForeignKey("GenderTypeId")]
    [InverseProperty("Customers")]
    public virtual GenderType? GenderType { get; set; }

    /// <summary>
    /// Inverse collection of Group created by user 
    /// </summary>
    [InverseProperty("CreatedByCustomer")]
    public virtual ICollection<Group> GroupCreatedByCustomers { get; set; } = new List<Group>();

    /// <summary>
    /// Inverse collection of GroupEffectiveMember that was created by user
    /// </summary>
    [InverseProperty("CreatedByCustomer")]
    public virtual ICollection<GroupEffectiveMember> GroupEffectiveMemberCreatedByCustomers { get; set; } = new List<GroupEffectiveMember>();

    /// <summary>
    /// Inverse collection of group effective member that was updated by user
    /// </summary>
    [InverseProperty("UpdatedByCustomer")]
    public virtual ICollection<GroupEffectiveMember> GroupEffectiveMemberUpdatedByCustomers { get; set; } = new List<GroupEffectiveMember>();

    /// <summary>
    /// Inverse collection of GroupPermission that was created by user
    /// </summary>
    [InverseProperty("CreatedByCustomer")]
    public virtual ICollection<GroupPermission> GroupPermissionCreatedByCustomers { get; set; } = new List<GroupPermission>();

    /// <summary>
    /// Inverse collection of GroupPermission that was updated by user
    /// </summary>
    [InverseProperty("UpdatedByCustomer")]
    public virtual ICollection<GroupPermission> GroupPermissionUpdatedByCustomers { get; set; } = new List<GroupPermission>();

    /// <summary>
    /// Inverse collection of Group that was updated by user
    /// </summary>
    [InverseProperty("UpdatedByCustomer")]
    public virtual ICollection<Group> GroupUpdatedByCustomers { get; set; } = new List<Group>();

    /// <summary>
    /// Link to dbo.HealthCareInsurer 
    /// </summary>
    [ForeignKey("HealthCareInsurerId")]
    [InverseProperty("Customers")]
    public virtual HealthCareInsurer? HealthCareInsurer { get; set; }

    /// <summary>
    /// Inverse collection of HealthCareInsurer that was created by user
    /// </summary>
    [InverseProperty("CreatedByCustomer")]
    public virtual ICollection<HealthCareInsurer> HealthCareInsurerCreatedByCustomers { get; set; } = new List<HealthCareInsurer>();

    /// <summary>
    /// Inverse collection of HealthCareInsurer that was updated by user
    /// </summary>
    [InverseProperty("UpdatedByCustomer")]
    public virtual ICollection<HealthCareInsurer> HealthCareInsurerUpdatedByCustomers { get; set; } = new List<HealthCareInsurer>();

    /// <summary>
    /// Inverse collection of Customer that was created by user
    /// </summary>
    [InverseProperty("CreatedByCustomer")]
    public virtual ICollection<Customer> InverseCreatedByCustomer { get; set; } = new List<Customer>();

    /// <summary>
    /// Inverse collection of Customer that was updated by user
    /// </summary>
    [InverseProperty("UpdatedByCustomer")]
    public virtual ICollection<Customer> InverseUpdatedByCustomer { get; set; } = new List<Customer>();

    /// <summary>
    /// Link to dbo.Organization as an organization where user was created
    /// </summary>
    [ForeignKey("OrganizationId")]
    [InverseProperty("Customers")]
    public virtual Organization? Organization { get; set; }

    /// <summary>
    /// Link to dbo.PasswordFormatType as a format type of saved users password
    /// </summary>
    [ForeignKey("PasswordFormatTypeId")]
    [InverseProperty("Customers")]
    public virtual PasswordFormatType PasswordFormatType { get; set; } = null!;

    /// <summary>
    /// Inverse collection of PermissionCategory that was created by user
    /// </summary>
    [InverseProperty("CreatedByCustomer")]
    public virtual ICollection<PermissionCategory> PermissionCategoryCreatedByCustomers { get; set; } = new List<PermissionCategory>();

    /// <summary>
    /// Inverse collection of PermissionCategory that was updated by user
    /// </summary>
    [InverseProperty("UpdatedByCustomer")]
    public virtual ICollection<PermissionCategory> PermissionCategoryUpdatedByCustomers { get; set; } = new List<PermissionCategory>();

    /// <summary>
    /// Inverse collection of Permission that was created by user
    /// </summary>
    [InverseProperty("CreatedByCustomer")]
    public virtual ICollection<Permission> PermissionCreatedByCustomers { get; set; } = new List<Permission>();

    /// <summary>
    /// Inverse collection of Permission that was updated by user
    /// </summary>
    [InverseProperty("UpdatedByCustomer")]
    public virtual ICollection<Permission> PermissionUpdatedByCustomers { get; set; } = new List<Permission>();

    /// <summary>
    /// Link to dbo.Picture as a photo of a user
    /// </summary>
    [ForeignKey("PictureId")]
    [InverseProperty("Customers")]
    public virtual Picture? Picture { get; set; }

    /// <summary>
    /// Inverse collection of Picture that was created by user
    /// </summary>
    [InverseProperty("CreatedByCustomer")]
    public virtual ICollection<Picture> PictureCreatedByCustomers { get; set; } = new List<Picture>();

    /// <summary>
    /// Inverse collection of Picture that was updated by user
    /// </summary>
    [InverseProperty("UpdatedByCustomer")]
    public virtual ICollection<Picture> PictureUpdatedByCustomers { get; set; } = new List<Picture>();

    /// <summary>
    /// Inverse collection of users Pictures
    /// </summary>
    [InverseProperty("User")]
    public virtual ICollection<Picture> PictureUsers { get; set; } = new List<Picture>();

    /// <summary>
    /// Link to dbo.ProfessionType as a profession of user
    /// </summary>
    [ForeignKey("ProfessionTypeId")]
    [InverseProperty("Customers")]
    public virtual ProfessionType? ProfessionType { get; set; }

    /// <summary>
    /// Inverse collection of ProffesionType created by user
    /// </summary>
    [InverseProperty("CreatedByCustomer")]
    public virtual ICollection<ProfessionType> ProfessionTypeCreatedByCustomers { get; set; } = new List<ProfessionType>();

    /// <summary>
    /// Inverse collection of ProffesionType updated by user
    /// </summary>
    [InverseProperty("UpdatedByCustomer")]
    public virtual ICollection<ProfessionType> ProfessionTypeUpdatedByCustomers { get; set; } = new List<ProfessionType>();

    /// <summary>
    /// Inverse collection of Provider created by user
    /// </summary>
    [InverseProperty("CreatedByCustomer")]
    public virtual ICollection<Provider> ProviderCreatedByCustomers { get; set; } = new List<Provider>();

    /// <summary>
    /// Inverse collection of Provider updated by user
    /// </summary>
    [InverseProperty("UpdatedByCustomer")]
    public virtual ICollection<Provider> ProviderUpdatedByCustomers { get; set; } = new List<Provider>();

    /// <summary>
    /// Inverse collection of RoleCategoryCombination created by user
    /// </summary>
    [InverseProperty("CreatedByCustomer")]
    public virtual ICollection<RoleCategoryCombination> RoleCategoryCombinationCreatedByCustomers { get; set; } = new List<RoleCategoryCombination>();

    /// <summary>
    /// Inverse collection of RoleCategoryCombination updated by user
    /// </summary>
    [InverseProperty("UpdatedByCustomer")]
    public virtual ICollection<RoleCategoryCombination> RoleCategoryCombinationUpdatedByCustomers { get; set; } = new List<RoleCategoryCombination>();

    /// <summary>
    /// Inverse collection of RoleCategory created by user
    /// </summary>
    [InverseProperty("CreatedByCustomer")]
    public virtual ICollection<RoleCategory> RoleCategoryCreatedByCustomers { get; set; } = new List<RoleCategory>();

    /// <summary>
    /// Inverse collection of RoleCategory updated by user
    /// </summary>
    [InverseProperty("UpdatedByCustomer")]
    public virtual ICollection<RoleCategory> RoleCategoryUpdatedByCustomers { get; set; } = new List<RoleCategory>();

    /// <summary>
    /// Inverse collection of Role created by user
    /// </summary>
    [InverseProperty("CreatedByCustomer")]
    public virtual ICollection<Role> RoleCreatedByCustomers { get; set; } = new List<Role>();

    /// <summary>
    /// Inverse collection of RoleMember created by user
    /// </summary>
    [InverseProperty("CreatedByCustomer")]
    public virtual ICollection<RoleMember> RoleMemberCreatedByCustomers { get; set; } = new List<RoleMember>();

    /// <summary>
    /// Inverse collection of RoleMember of DirectUsers (those with administrative role)
    /// </summary>
    [InverseProperty("DirectUser")]
    public virtual ICollection<RoleMember> RoleMemberDirectUsers { get; set; } = new List<RoleMember>();

    /// <summary>
    /// Inverse collection of RoleMember updated by user
    /// </summary>
    [InverseProperty("UpdatedByCustomer")]
    public virtual ICollection<RoleMember> RoleMemberUpdatedByCustomers { get; set; } = new List<RoleMember>();

    /// <summary>
    /// Inverse collection of RolePermission created by user
    /// </summary>
    [InverseProperty("CreatedByCustomer")]
    public virtual ICollection<RolePermission> RolePermissionCreatedByCustomers { get; set; } = new List<RolePermission>();

    /// <summary>
    /// Inverse collection of RolePermission updated by user
    /// </summary>
    [InverseProperty("UpdatedByCustomer")]
    public virtual ICollection<RolePermission> RolePermissionUpdatedByCustomers { get; set; } = new List<RolePermission>();

    /// <summary>
    /// Inverse collection of RoleSubCategory created by user
    /// </summary>
    [InverseProperty("CreatedByCustomer")]
    public virtual ICollection<RoleSubCategory> RoleSubCategoryCreatedByCustomers { get; set; } = new List<RoleSubCategory>();

    /// <summary>
    /// Inverse collection of RoleSubCategory updated by user
    /// </summary>
    [InverseProperty("UpdatedByCustomer")]
    public virtual ICollection<RoleSubCategory> RoleSubCategoryUpdatedByCustomers { get; set; } = new List<RoleSubCategory>();

    /// <summary>
    /// Inverse collection of Role updated by user
    /// </summary>
    [InverseProperty("UpdatedByCustomer")]
    public virtual ICollection<Role> RoleUpdatedByCustomers { get; set; } = new List<Role>();

    /// <summary>
    /// Inverse collection of Subject (or modules) allowed to organization created by user
    /// </summary>
    [InverseProperty("CreatedByCustomer")]
    public virtual ICollection<SubjectAllowedToOrganization> SubjectAllowedToOrganizationCreatedByCustomers { get; set; } = new List<SubjectAllowedToOrganization>();

    /// <summary>
    /// Inverse collection of SubjectAllowedToOrganization updated by user
    /// </summary>
    [InverseProperty("UpdatedByCustomer")]
    public virtual ICollection<SubjectAllowedToOrganization> SubjectAllowedToOrganizationUpdatedByCustomers { get; set; } = new List<SubjectAllowedToOrganization>();

    /// <summary>
    /// Inverse collection of SubjectAllowedToProvider created by user
    /// </summary>
    [InverseProperty("CreatedByCustomer")]
    public virtual ICollection<SubjectAllowedToProvider> SubjectAllowedToProviderCreatedByCustomers { get; set; } = new List<SubjectAllowedToProvider>();

    /// <summary>
    /// Inverse collection of SubjectAllowedToProvider updated by user
    /// </summary>
    [InverseProperty("UpdatedByCustomer")]
    public virtual ICollection<SubjectAllowedToProvider> SubjectAllowedToProviderUpdatedByCustomers { get; set; } = new List<SubjectAllowedToProvider>();

    /// <summary>
    /// Inverse collection of Subject created by user
    /// </summary>
    [InverseProperty("CreatedByCustomer")]
    public virtual ICollection<Subject> SubjectCreatedByCustomers { get; set; } = new List<Subject>();

    /// <summary>
    /// Inverse collection of Subject updated by user
    /// </summary>
    [InverseProperty("UpdatedByCustomer")]
    public virtual ICollection<Subject> SubjectUpdatedByCustomers { get; set; } = new List<Subject>();

    /// <summary>
    /// Link to dbo.Customer as a customer who was updated by customer
    /// </summary>
    [ForeignKey("UpdatedByCustomerId")]
    [InverseProperty("InverseUpdatedByCustomer")]
    public virtual Customer? UpdatedByCustomer { get; set; }

    /// <summary>
    /// Inverse collection of UserPermission created by user
    /// </summary>
    [InverseProperty("CreatedByCustomer")]
    public virtual ICollection<UserPermission> UserPermissionCreatedByCustomers { get; set; } = new List<UserPermission>();

    /// <summary>
    /// Inverse collection of UserPermission updated by user
    /// </summary>
    [InverseProperty("UpdatedByCustomer")]
    public virtual ICollection<UserPermission> UserPermissionUpdatedByCustomers { get; set; } = new List<UserPermission>();

    /// <summary>
    /// Inverse collection of users UserPermission
    /// </summary>
    [InverseProperty("User")]
    public virtual ICollection<UserPermission> UserPermissionUsers { get; set; } = new List<UserPermission>();
}
