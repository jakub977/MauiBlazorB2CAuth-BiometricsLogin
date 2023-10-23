using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using Principal.Telemedicine.DataConnectors.Models.Shared;
using Principal.Telemedicine.Shared.Models;

namespace Principal.Telemedicine.DataConnectors.Mapping;

/// <summary>
/// Mapper objektu na objekt.
/// </summary>
public class Mapping : Profile
{
    public Mapping()
    {
        CreateMap<Customer, UserContract>()
            .ForMember(x => x.Active, opt => opt.Ignore())
            .ForMember(x => x.Deleted, opt => opt.Ignore())
            .ForMember(x => x.CreatedByProviderId, opt => opt.Ignore())
            .ForMember(x => x.CreatedByCustomerId, opt => opt.Ignore())
            .ForMember(x => x.CreatedDateUtc, opt => opt.Ignore())
            .ForMember(x => x.UpdatedByCustomerId, opt => opt.Ignore())
            .ForMember(x => x.UpdateDateUtc, opt => opt.Ignore())
            .ForMember(x => x.GenderTypeId, opt => opt.Ignore())
            .ForMember(x => x.PictureId, opt => opt.Ignore())
            .ForMember(x => x.AdminComment, opt => opt.Ignore())
            .ForMember(x => x.Password, opt => opt.Ignore())
            .ForMember(x => x.PasswordFormatTypeId, opt => opt.Ignore())
            .ForMember(x => x.PasswordSalt, opt => opt.Ignore())
            .ForMember(x => x.LastIpAddress, opt => opt.Ignore())
            .ForMember(x => x.LastLoginDateUtc, opt => opt.Ignore())
            .ForMember(x => x.LastActivityDateUtc, opt => opt.Ignore())
            .ForMember(x => x.InvalidLoginsCount, opt => opt.Ignore())
            .ForMember(x => x.ApiloginToken, opt => opt.Ignore())
            .ForMember(x => x.LastApiloginDateTime, opt => opt.Ignore())
            .ForMember(x => x.ApiloginEnabled, opt => opt.Ignore())
            .ForMember(x => x.ProfessionTypeId, opt => opt.Ignore())
            .ForMember(x => x.EmployerName, opt => opt.Ignore())
            .ForMember(x => x.Note, opt => opt.Ignore())
            .ForMember(x => x.HealthCareInsurerId, opt => opt.Ignore())
            .ForMember(x => x.BirthIdentificationNumber, opt => opt.Ignore())
            .ForMember(x => x.IsRiskPatient, opt => opt.Ignore())
            .ForMember(x => x.AppInstanceToken, opt => opt.Ignore());

        CreateMap<Provider, ProviderContract>();
        CreateMap<Role, RoleContract>();
        CreateMap<Role, RoleProviderContract>();
        CreateMap<RoleCategory, RoleCategoryContract>();
        CreateMap<RoleCategoryCombination, RoleCategoryCombinationContract>();
        CreateMap<RolePermission, RolePermissionContract>();
        CreateMap<RoleSubCategory, RoleSubCategoryContract>();
        CreateMap<RoleMember, RoleMemberContract>();
        CreateMap<RoleMember, RoleMemberProviderContract>();
        CreateMap<GroupEffectiveMember, GroupEffectiveMemberContract>();
        CreateMap<Group, GroupContract>();
        CreateMap<GroupPermission, GroupPermissionContract>();
        CreateMap<Organization, OrganizationContract>();
        CreateMap<Permission, PermissionContract>();
        CreateMap<PermissionCategory, PermissionCategoryContract>();
        CreateMap<PermissionType, PermissionTypeContract>();
        CreateMap<Picture, PictureContract>();
        CreateMap<Subject, SubjectContract>();
        CreateMap<SubjectAllowedToOrganization, SubjectAllowedToOrganizationContract>();
        CreateMap<SubjectAllowedToProvider, SubjectAllowedToProviderContract>();
        CreateMap<SubjectType, SubjectTypeContract>();
        CreateMap<UserPermission, UserPermissionContract>();
        CreateMap<EffectiveUser, EffectiveUserContract>();
        CreateMap<EffectiveUser, EffectiveUserProviderContract>();
        CreateMap<GenderType, GenderTypeContract>();
        CreateMap<HealthCareInsurer, HealthCareInsurerContract>();
        CreateMap<PasswordFormatType, PasswordFormatTypeContract>();
        CreateMap<ProfessionType, ProfessionTypeContract>();
        CreateMap<AddressCity, AddressCityContract>();
        CreateMap<MediaStorage, MediaStorageContract>();

        CreateMap<Customer, CompleteUserContract>()
           .ForMember(x => x.AdminComment, opt => opt.Ignore())
           .ForMember(x => x.Password, opt => opt.Ignore())
           .ForMember(x => x.PasswordSalt, opt => opt.Ignore())
           .ForMember(x => x.LastIpAddress, opt => opt.Ignore())
           .ForMember(x => x.LastLoginDateUtc, opt => opt.Ignore())
           .ForMember(x => x.LastActivityDateUtc, opt => opt.Ignore())
           .ForMember(x => x.InvalidLoginsCount, opt => opt.Ignore())
           .ForMember(x => x.ApiloginToken, opt => opt.Ignore())
           .ForMember(x => x.LastApiloginDateTime, opt => opt.Ignore())
           .ForMember(x => x.ApiloginEnabled, opt => opt.Ignore())
           .ForMember(x => x.ProfessionName, opt => opt.Ignore())
           .ForMember(x => x.ProfessionTypeId, opt => opt.Ignore())
           .ForMember(x => x.Note, opt => opt.Ignore())
           .ForMember(x => x.IsRiskPatient, opt => opt.Ignore())
           .ForMember(x => x.GenderType, opt => opt.Ignore())
           .ForMember(x => x.PublicIdentifier, opt => opt.Ignore())
           .ForMember(x => x.PasswordFormatTypeId, opt => opt.Ignore());

        CreateMap<EffectiveUserContract, EffectiveUser>();
        CreateMap<UserPermissionContract, UserPermission>();
        CreateMap<PictureContract, Picture>();
        CreateMap<MediaStorageContract, MediaStorage>();
        CreateMap<OrganizationContract, Organization>();
        CreateMap<RoleContract, Role>();
        CreateMap<ProviderContract, Provider>();
        CreateMap<RoleMemberContract, RoleMember>();
        CreateMap<RoleCategoryContract, RoleCategory>();
        CreateMap<RoleCategoryCombinationContract, RoleCategoryCombination>();
        CreateMap<RolePermissionContract, RolePermission>();
        CreateMap<RoleSubCategoryContract, RoleSubCategory>();
        CreateMap<GroupEffectiveMemberContract, GroupEffectiveMember>();
        CreateMap<GroupContract, Group>();
        CreateMap<GroupPermissionContract, GroupPermission>();
        CreateMap<PermissionContract, Permission>();
        CreateMap<PermissionCategoryContract, PermissionCategory>();
        CreateMap<PermissionTypeContract, PermissionType>();
        CreateMap<SubjectContract, Subject>();
        CreateMap<SubjectAllowedToOrganizationContract, SubjectAllowedToOrganization>();
        CreateMap<SubjectAllowedToProviderContract, SubjectAllowedToProvider>();
        CreateMap<SubjectTypeContract, SubjectType>();
        CreateMap<GenderTypeContract, GenderType>();
        CreateMap<HealthCareInsurerContract, HealthCareInsurer>();
        CreateMap<PasswordFormatTypeContract, PasswordFormatType>();
        CreateMap<ProfessionTypeContract, ProfessionType>();
    }
}
