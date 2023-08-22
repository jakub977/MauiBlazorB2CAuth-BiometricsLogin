using System.Runtime.Serialization;
//TOD Files Scoped
namespace Principal.Telemedicine.Shared.Models
{
    //TODO Popis
    [DataContract]
    public class UserContract 
    {
        [DataMember]
        public int Id { get; set; }

        
        public bool? Active { get; set; }

        
        public bool Deleted { get; set; }

        
        public int? CreatedByProviderId { get; set; }

       
        public int? CreatedByCustomerId { get; set; }

        
        public DateTime CreatedDateUtc { get; set; }

        
        public int? UpdatedByCustomerId { get; set; }

        
        public DateTime? UpdateDateUtc { get; set; }

        [DataMember]
        public int? OrganizationId { get; set; }

        
        [DataMember]
        public string FirstName { get; set; } = null!;

        [DataMember]
        public string LastName { get; set; } = null!;

        public int? GenderTypeId { get; set; }

        [DataMember]
        public string? AddressLine { get; set; }

        [DataMember]
        public string? PostalCode { get; set; }

        [DataMember]
        public string Email { get; set; } = null!;

        [DataMember]
        public string? TelephoneNumber { get; set; }

        [DataMember]
        public string? PersonalIdentificationNumber { get; set; }

        [DataMember]
        public DateTime? Birthdate { get; set; }

        [DataMember]
        public string? HealthCareInsurerCode { get; set; }

        [DataMember]
        public string? PublicIdentifier { get; set; }

        public int? PictureId { get; set; }

        [DataMember]
        public string? TitleBefore { get; set; }

        [DataMember]
        public string? TitleAfter { get; set; }

        [DataMember]
        public bool IsSystemAccount { get; set; }

        [DataMember]
        public bool IsSuperAdminAccount { get; set; }

        [DataMember]
        public bool IsOrganizationAdminAccount { get; set; }

        [DataMember]
        public bool IsProviderAdminAccount { get; set; }

        public string? AdminComment { get; set; }
        
        public string? Password { get; set; }
        
        public int PasswordFormatTypeId { get; set; }

        public string? PasswordSalt { get; set; }

        public string? LastIpAddress { get; set; }

        public DateTime? LastLoginDateUtc { get; set; }

        public DateTime? LastActivityDateUtc { get; set; }

        public int InvalidLoginsCount { get; set; }

        public string? ApiloginToken { get; set; }

        public DateTime? LastApiloginDateTime { get; set; }

        public bool? ApiloginEnabled { get; set; }

        [DataMember]
        public string FriendlyName { get; set; } = null!;

        [DataMember]
        public string GlobalId { get; set; } = null!;

        public int? ProfessionTypeId { get; set; }

        [DataMember]
        public string? ProfessionName { get; set; }
        
        public string? EmployerName { get; set; }

        public string? Note { get; set; }

        [DataMember]
        public string? TelephoneNumber2 { get; set; }

        public int? HealthCareInsurerId { get; set; }

        public string? BirthIdentificationNumber { get; set; }

        [DataMember]
        public string? Street { get; set; }

        [DataMember]
        public int? CityId { get; set; }

        public bool? IsRiskPatient { get; set; }

        
    }
}
