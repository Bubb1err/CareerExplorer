using CareerExplorer.Core.Entities;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace CareerExplorer.Web.DTO
{
    public class VacancyDTO 
    {
        public int Id { get; set; }
        [Required]
        [MinLength(200, ErrorMessage = "Provide at least 200 symbols.")]
        public string Description { get; set; }
        [Required]
        public bool IsAvailable { get; set; } = false;
        public bool IsAccepted { get; set; }
        public int CreatorId { get; set; }
        public string CreatorNickName { get; set; }
        public string CreatorName { get; set; }
        public string CreatorSurname { get; set; }
        public string CompanyName { get; set; }
        public string CompanyDesciprion { get; set; }
        public Position? Position { get; set; } 
        public ICollection<SkillsTag> Requirements { get; set; } = new List<SkillsTag>();
        public DateTime CreatedDate { get; set; } 
        public bool IsApplied { get; set; }
        public int Views { get; set; }
        public Country? Country { get; set; }
        public City? City { get; set; }
        public int? Salary { get; set; }
        public int? WorkType { get; set; }
        public int? EnglishLevel { get; set; }
        public int? ExperienceYears { get; set; }
    }
}
