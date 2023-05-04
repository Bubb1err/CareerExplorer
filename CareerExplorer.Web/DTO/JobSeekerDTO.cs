using CareerExplorer.Core.Entities;
using CareerExplorer.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareerExplorer.Web.DTO
{
    public class JobSeekerDTO
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Surname { get; set; }
        [Required]
        [Phone]
        public string? Phone { get; set; }
        [Required]
        [Range(0, 20000, ErrorMessage = "Provide number between 0 and 20 000.")]
        public int? Salary { get; set; }
        public string? GitHub { get; set; }
        public string? LinkedIn { get; set; }
        [Required]
        [MinLength(100, ErrorMessage =  "Experience must contain at least 100 symbols.")]
        public string? Experience { get; set; }
        public string? UserId { get; set; }
        public int? CountryId { get; set; }
        public Country? Country { get; set; }
        public int? CityId { get; set; }
        public City? City { get; set; }
        public int? DesiredPositionId { get; set; }
        public Position? DesiredPosition { get; set; }
        public int? ExperienceYears { get; set; }
        public ICollection<SkillsTag> Skills { get; set; } = new List<SkillsTag>();
        public int? EnglishLevel { get; set; }
        public bool IsFilled { get; set; }
        public bool IsAccepted { get; set; }
    }
}
