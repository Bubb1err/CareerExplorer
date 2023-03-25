using AutoMapper;
using CareerExplorer.Core.Entities;
using CareerExplorer.Web.DTO;

namespace CareerExplorer.Web
{
    public class MappingConfig : Profile
    {
        public MappingConfig() 
        { 
            CreateMap<Recruiter, RecruiterProfileDTO>().ReverseMap();
            CreateMap<Vacancy, VacancyDTO>().ReverseMap();
            CreateMap<JobSeeker, ApplicantDTO>().ReverseMap();
        }
    }
}
