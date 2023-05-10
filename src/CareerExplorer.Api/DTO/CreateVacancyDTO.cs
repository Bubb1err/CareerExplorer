using System.ComponentModel.DataAnnotations;

namespace CareerExplorer.Api.DTO
{
    public class CreateVacancyDTO
    {
        [Required]
        [MinLength(200, ErrorMessage = "Provide at least 200 symbols.")]
        public string Description { get; set; }
        [Required]
        public bool IsAvailable { get; set; }
        [Required]
        public int? CountryId { get; set; }
        [Required]
        public int? CityId { get; set; }
        [Required]
        [Range(0, 20000, ErrorMessage = "Provide number between 0 and 20 000.")]
        public int? Salary { get; set; }
        [Required]
        public int? WorkType { get; set; }
        [Required]
        public int? EnglishLevel { get; set; }
        [Required]
        public int? ExperienceYears { get; set; }
    }
}
