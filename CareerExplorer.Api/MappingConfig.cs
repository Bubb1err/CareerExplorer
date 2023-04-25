using AutoMapper;
using CareerExplorer.Api.DTO;
using CareerExplorer.Core.Entities;

namespace CareerExplorer.Api
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
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
            CreateMap<SkillsTag, SkillTagDTO>().ReverseMap();
            CreateMap<Position, PositionDTO>().ReverseMap();
        }
    }
}
