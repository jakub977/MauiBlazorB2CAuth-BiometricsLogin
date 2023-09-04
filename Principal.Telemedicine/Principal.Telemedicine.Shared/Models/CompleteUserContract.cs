using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Principal.Telemedicine.Shared.Models;

/// <summary>
/// User data contract derived from many cs.
/// </summary>
[DataContract]
public class CompleteUserContract : UserContract
{
    [ForeignKey("CityId")]
    [InverseProperty("Customers")]
    public virtual AddressCityContract? City { get; set; }

    [ForeignKey("CreatedByCustomerId")]
    [InverseProperty("InverseCreatedByCustomer")]
    public virtual UserContract? CreatedByCustomer { get; set; }

    [ForeignKey("CreatedByProviderId")]
    [InverseProperty("Customers")]
    public virtual ProviderContract? CreatedByProvider { get; set; }

    [InverseProperty("CreatedByCustomer")]
    public virtual ICollection<EffectiveUserContract> EffectiveUserCreatedByCustomers { get; set; } = new List<EffectiveUserContract>();

    [InverseProperty("UpdatedByCustomer")]
    public virtual ICollection<EffectiveUserContract> EffectiveUserUpdatedByCustomers { get; set; } = new List<EffectiveUserContract>();

    [InverseProperty("User")]
    [DataMember]
    public virtual ICollection<EffectiveUserContract> EffectiveUserUsers { get; set; } = new List<EffectiveUserContract>();

    [ForeignKey("GenderTypeId")]
    [InverseProperty("Customers")]
    public virtual GenderTypeContract? GenderType { get; set; }

    [InverseProperty("CreatedByCustomer")]
    public virtual ICollection<GroupContract> GroupCreatedByCustomers { get; set; } = new List<GroupContract>();

    [InverseProperty("CreatedByCustomer")]
    public virtual ICollection<GroupEffectiveMemberContract> GroupEffectiveMemberCreatedByCustomers { get; set; } = new List<GroupEffectiveMemberContract>();

    [InverseProperty("UpdatedByCustomer")]
    public virtual ICollection<GroupEffectiveMemberContract> GroupEffectiveMemberUpdatedByCustomers { get; set; } = new List<GroupEffectiveMemberContract>();

    [InverseProperty("CreatedByCustomer")]
    public virtual ICollection<GroupPermissionContract> GroupPermissionCreatedByCustomers { get; set; } = new List<GroupPermissionContract>();

    [InverseProperty("UpdatedByCustomer")]
    public virtual ICollection<GroupPermissionContract> GroupPermissionUpdatedByCustomers { get; set; } = new List<GroupPermissionContract>();

    [InverseProperty("UpdatedByCustomer")]
    public virtual ICollection<GroupContract> GroupUpdatedByCustomers { get; set; } = new List<GroupContract>();

    [ForeignKey("HealthCareInsurerId")]
    [InverseProperty("Customers")]
    public virtual HealthCareInsurerContract? HealthCareInsurer { get; set; }

    [InverseProperty("CreatedByCustomer")]
    public virtual ICollection<HealthCareInsurerContract> HealthCareInsurerCreatedByCustomers { get; set; } = new List<HealthCareInsurerContract>();

    [InverseProperty("UpdatedByCustomer")]
    public virtual ICollection<HealthCareInsurerContract> HealthCareInsurerUpdatedByCustomers { get; set; } = new List<HealthCareInsurerContract>();

    [InverseProperty("CreatedByCustomer")]
    public virtual ICollection<UserContract> InverseCreatedByCustomer { get; set; } = new List<UserContract>();

    [InverseProperty("UpdatedByCustomer")]
    public virtual ICollection<UserContract> InverseUpdatedByCustomer { get; set; } = new List<UserContract>();

    [ForeignKey("OrganizationId")]
    [InverseProperty("Customers")]
    public virtual OrganizationContract? Organization { get; set; }

    [ForeignKey("PasswordFormatTypeId")]
    [InverseProperty("Customers")]
    public virtual PasswordFormatTypeContract PasswordFormatType { get; set; } = null!;

    [InverseProperty("CreatedByCustomer")]
    public virtual ICollection<PermissionCategoryContract> PermissionCategoryCreatedByCustomers { get; set; } = new List<PermissionCategoryContract>();

    [InverseProperty("UpdatedByCustomer")]
    public virtual ICollection<PermissionCategoryContract> PermissionCategoryUpdatedByCustomers { get; set; } = new List<PermissionCategoryContract>();

    [InverseProperty("CreatedByCustomer")]
    public virtual ICollection<PermissionContract> PermissionCreatedByCustomers { get; set; } = new List<PermissionContract>();

    [InverseProperty("UpdatedByCustomer")]
    public virtual ICollection<PermissionContract> PermissionUpdatedByCustomers { get; set; } = new List<PermissionContract>();

    [ForeignKey("PictureId")]
    [InverseProperty("Customers")]
    public virtual PictureContract? Picture { get; set; }

    [InverseProperty("CreatedByCustomer")]
    public virtual ICollection<PictureContract> PictureCreatedByCustomers { get; set; } = new List<PictureContract>();

    [InverseProperty("UpdatedByCustomer")]
    public virtual ICollection<PictureContract> PictureUpdatedByCustomers { get; set; } = new List<PictureContract>();

    [InverseProperty("User")]
    public virtual ICollection<PictureContract> PictureUsers { get; set; } = new List<PictureContract>();

    [ForeignKey("ProfessionTypeId")]
    [InverseProperty("Customers")]
    public virtual ProfessionTypeContract? ProfessionType { get; set; }

    [InverseProperty("CreatedByCustomer")]
    public virtual ICollection<ProfessionTypeContract> ProfessionTypeCreatedByCustomers { get; set; } = new List<ProfessionTypeContract>();

    [InverseProperty("UpdatedByCustomer")]
    public virtual ICollection<ProfessionTypeContract> ProfessionTypeUpdatedByCustomers { get; set; } = new List<ProfessionTypeContract>();

    [InverseProperty("CreatedByCustomer")]
    public virtual ICollection<ProviderContract> ProviderCreatedByCustomers { get; set; } = new List<ProviderContract>();

    [InverseProperty("UpdatedByCustomer")]
    public virtual ICollection<ProviderContract> ProviderUpdatedByCustomers { get; set; } = new List<ProviderContract>();

    [DataMember]
    [InverseProperty("CreatedByCustomer")]
    public virtual ICollection<RoleCategoryCombinationContract> RoleCategoryCombinationCreatedByCustomers { get; set; } = new List<RoleCategoryCombinationContract>();

    [DataMember]
    [InverseProperty("UpdatedByCustomer")]
    public virtual ICollection<RoleCategoryCombinationContract> RoleCategoryCombinationUpdatedByCustomers { get; set; } = new List<RoleCategoryCombinationContract>();
    
    [DataMember]
    [InverseProperty("CreatedByCustomer")]
    public virtual ICollection<RoleCategoryContract> RoleCategoryCreatedByCustomers { get; set; } = new List<RoleCategoryContract>();
    
    [DataMember]
    [InverseProperty("UpdatedByCustomer")]
    public virtual ICollection<RoleCategoryContract> RoleCategoryUpdatedByCustomers { get; set; } = new List<RoleCategoryContract>();
   
    [DataMember]
    [InverseProperty("CreatedByCustomer")]
    public virtual ICollection<RoleContract> RoleCreatedByCustomers { get; set; } = new List<RoleContract>();

    [DataMember]
    [InverseProperty("CreatedByCustomer")]
    public virtual ICollection<RoleMemberContract> RoleMemberCreatedByCustomers { get; set; } = new List<RoleMemberContract>();
    
    [DataMember]
    [InverseProperty("DirectUser")]
    public virtual ICollection<RoleMemberContract> RoleMemberDirectUsers { get; set; } = new List<RoleMemberContract>();

    [DataMember]
    [InverseProperty("UpdatedByCustomer")]
    public virtual ICollection<RoleMemberContract> RoleMemberUpdatedByCustomers { get; set; } = new List<RoleMemberContract>();

    [DataMember]
    [InverseProperty("CreatedByCustomer")]
    public virtual ICollection<RolePermissionContract> RolePermissionCreatedByCustomers { get; set; } = new List<RolePermissionContract>();

    [DataMember]
    [InverseProperty("UpdatedByCustomer")]
    public virtual ICollection<RolePermissionContract> RolePermissionUpdatedByCustomers { get; set; } = new List<RolePermissionContract>();

    [DataMember]
    [InverseProperty("CreatedByCustomer")]
    public virtual ICollection<RoleSubCategoryContract> RoleSubCategoryCreatedByCustomers { get; set; } = new List<RoleSubCategoryContract>();

    [DataMember]
    [InverseProperty("UpdatedByCustomer")]
    public virtual ICollection<RoleSubCategoryContract> RoleSubCategoryUpdatedByCustomers { get; set; } = new List<RoleSubCategoryContract>();

    [DataMember]
    [InverseProperty("UpdatedByCustomer")]
    public virtual ICollection<RoleContract> RoleUpdatedByCustomers { get; set; } = new List<RoleContract>();

    [InverseProperty("CreatedByCustomer")]
    public virtual ICollection<SubjectAllowedToOrganizationContract> SubjectAllowedToOrganizationCreatedByCustomers { get; set; } = new List<SubjectAllowedToOrganizationContract>();

    [InverseProperty("UpdatedByCustomer")]
    public virtual ICollection<SubjectAllowedToOrganizationContract> SubjectAllowedToOrganizationUpdatedByCustomers { get; set; } = new List<SubjectAllowedToOrganizationContract>();

    [InverseProperty("CreatedByCustomer")]
    public virtual ICollection<SubjectAllowedToProviderContract> SubjectAllowedToProviderCreatedByCustomers { get; set; } = new List<SubjectAllowedToProviderContract>();

    [InverseProperty("UpdatedByCustomer")]
    public virtual ICollection<SubjectAllowedToProviderContract> SubjectAllowedToProviderUpdatedByCustomers { get; set; } = new List<SubjectAllowedToProviderContract>();

    [InverseProperty("CreatedByCustomer")]
    public virtual ICollection<SubjectContract> SubjectCreatedByCustomers { get; set; } = new List<SubjectContract>();

    [InverseProperty("UpdatedByCustomer")]
    public virtual ICollection<SubjectContract> SubjectUpdatedByCustomers { get; set; } = new List<SubjectContract>();

    [ForeignKey("UpdatedByCustomerId")]
    [InverseProperty("InverseUpdatedByCustomer")]
    public virtual UserContract? UpdatedByCustomer { get; set; }

    [DataMember]
    [InverseProperty("CreatedByCustomer")]
    public virtual ICollection<UserPermissionContract> UserPermissionCreatedByCustomers { get; set; } = new List<UserPermissionContract>();
   
    [DataMember]
    [InverseProperty("UpdatedByCustomer")]
    public virtual ICollection<UserPermissionContract> UserPermissionUpdatedByCustomers { get; set; } = new List<UserPermissionContract>();

    [DataMember]
    [InverseProperty("User")]
    public virtual ICollection<UserPermissionContract> UserPermissionUsers { get; set; } = new List<UserPermissionContract>();
}
