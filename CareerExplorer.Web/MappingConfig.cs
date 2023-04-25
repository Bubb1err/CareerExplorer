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
            CreateMap<Vacancy, VacancyDTO>()
                .ForMember(x => x.CompanyName,
                m => m.MapFrom(a => a.Creator.Company))
                .ForMember(x => x.CompanyDesciprion,
                m => m.MapFrom(a => a.Creator.CompanyDescription))
                .ForMember(x => x.CreatorSurname, 
                m => m.MapFrom(a => a.Creator.Surname))
                .ForMember(x => x.CreatorName,
                m => m.MapFrom(a => a.Creator.Name))
                .ForMember(x => x.CreatorNickName,
                m => m.MapFrom(a => a.Creator.AppUser.Email))
                .ReverseMap();
            CreateMap<JobSeeker, ApplicantDTO>().ReverseMap();
            CreateMap<Vacancy, EditVacancyDTO>().ReverseMap();
            CreateMap<SkillTagDTO, SkillsTag>().ReverseMap();
            CreateMap<Position, PositionDTO>().ReverseMap();
            CreateMap<JobSeeker, JobSeekerDTO>().ReverseMap();
            CreateMap<JobSeeker, JobSeekerViewProfileDTO > ()
                .ForMember(x => x.NickName,
                m => m.MapFrom(a => a.AppUser.Email))
                .ReverseMap();
            CreateMap<CreateVacancyDTO, Vacancy>().ReverseMap();
        }
    }
}
