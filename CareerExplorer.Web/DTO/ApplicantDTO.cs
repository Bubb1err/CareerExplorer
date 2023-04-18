using CareerExplorer.Core.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
namespace CareerExplorer.Web.DTO
{
    public class ApplicantDTO 
    {
        public int Id { get; set; }
        [BindNever]
        public int VacancyId { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Phone { get; set; }
        public string? GitHub { get; set; }
        public string? LinkedIn { get; set; }
        public string? Experience { get; set; }
        public Position? DesiredPosition { get; set; }
        public int? ExperienceYears { get; set; }
        public int? EnglishLevel { get; set; }
        public Country? Country { get; set; }
        public City? City { get; set; }
        public ICollection<SkillsTag> Skills { get; set; } = new List<SkillsTag>();
    }
}
