﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Principal.Telemedicine.DataConnectors.Models;
using Principal.Telemedicine.Shared.Models;

namespace Principal.Telemedicine.DataConnectors.Mapping
{
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


        }
    }
}