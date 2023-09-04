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
           .ForMember(x => x.IsRiskPatient, opt => opt.Ignore());

        CreateMap<Customer, CompleteUserContract>()
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
           .ForMember(x => x.City, opt => opt.Ignore())
           .ForMember(x => x.CreatedByCustomer, opt => opt.Ignore())
           .ForMember(x => x.CreatedByProvider, opt => opt.Ignore())
           .ForMember(x => x.GenderType, opt => opt.Ignore())
           .ForMember(x => x.GroupPermissionCreatedByCustomers, opt => opt.Ignore())
           .ForMember(x => x.GroupPermissionUpdatedByCustomers, opt => opt.Ignore())
           .ForMember(x => x.GroupUpdatedByCustomers, opt => opt.Ignore())
           .ForMember(x => x.HealthCareInsurer, opt => opt.Ignore())
           .ForMember(x => x.HealthCareInsurerCreatedByCustomers, opt => opt.Ignore())
           .ForMember(x => x.InverseCreatedByCustomer, opt => opt.Ignore())
           .ForMember(x => x.InverseUpdatedByCustomer, opt => opt.Ignore())
           .ForMember(x => x.PasswordFormatType, opt => opt.Ignore())
           .ForMember(x => x.PermissionCategoryCreatedByCustomers, opt => opt.Ignore())
           .ForMember(x => x.PermissionCategoryUpdatedByCustomers, opt => opt.Ignore())
           .ForMember(x => x.PermissionCreatedByCustomers, opt => opt.Ignore())
           .ForMember(x => x.PermissionUpdatedByCustomers, opt => opt.Ignore())
           .ForMember(x => x.Picture, opt => opt.Ignore())
           .ForMember(x => x.PictureCreatedByCustomers, opt => opt.Ignore())
           .ForMember(x => x.PictureUpdatedByCustomers, opt => opt.Ignore())
           .ForMember(x => x.PictureUsers, opt => opt.Ignore())
           .ForMember(x => x.ProfessionType, opt => opt.Ignore())

           .ForMember(x => x.ProfessionTypeCreatedByCustomers, opt => opt.Ignore())
           .ForMember(x => x.ProfessionTypeUpdatedByCustomers, opt => opt.Ignore())
           .ForMember(x => x.ProviderCreatedByCustomers, opt => opt.Ignore())
           .ForMember(x => x.ProviderUpdatedByCustomers, opt => opt.Ignore())
           .ForMember(x => x.SubjectAllowedToOrganizationCreatedByCustomers, opt => opt.Ignore())
           .ForMember(x => x.SubjectAllowedToOrganizationUpdatedByCustomers, opt => opt.Ignore())
           .ForMember(x => x.SubjectAllowedToProviderCreatedByCustomers, opt => opt.Ignore())
           .ForMember(x => x.SubjectAllowedToProviderUpdatedByCustomers, opt => opt.Ignore())
           .ForMember(x => x.SubjectCreatedByCustomers, opt => opt.Ignore())
           .ForMember(x => x.SubjectUpdatedByCustomers, opt => opt.Ignore())
           .ForMember(x => x.UpdatedByCustomer, opt => opt.Ignore())
            .ForMember(x => x.EffectiveUserCreatedByCustomers, opt => opt.Ignore())
            .ForMember(x => x.EffectiveUserUpdatedByCustomers, opt => opt.Ignore())
            .ForMember(x => x.EffectiveUserUpdatedByCustomers, opt => opt.Ignore())
            .ForMember(x => x.GroupCreatedByCustomers, opt => opt.Ignore())
            .ForMember(x => x.GroupEffectiveMemberCreatedByCustomers, opt => opt.Ignore())
            .ForMember(x => x.GroupEffectiveMemberUpdatedByCustomers, opt => opt.Ignore());





    }
}
