﻿using AutoMapper;
using CareerExplorer.Core.Entities;
using CareerExplorer.Web.DTO;

namespace CareerExplorer.Web
{
    public class MappingConfig : Profile
    {
        public MappingConfig() 
        { 
            CreateMap<Recruiter, RecruiterProfileDTO>().ReverseMap();
            CreateMap<Vacancy, VacancyDTO>()
                .ForMember(x => x.CompanyName,
                m => m.MapFrom(a => a.Creator.Company))
                .ForMember(x => x.CompanyDesciprion,
                m => m.MapFrom(a => a.Creator.CompanyDescription))
                .ForMember(x => x.CreatorSurname, 
                m => m.MapFrom(a => a.Creator.Surname))
                .ForMember(x => x.CreatorName,
                m => m.MapFrom(a => a.Creator.Name))
                .ReverseMap();
            CreateMap<JobSeeker, ApplicantDTO>().ReverseMap();
            CreateMap<Vacancy, CreateOrEditVacancyDTO>().ReverseMap();
        }
    }
}
