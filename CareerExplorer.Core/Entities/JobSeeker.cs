
namespace CareerExplorer.Core.Entities
{
    public class JobSeeker
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? Salary { get; set; }
        public string? Surname { get; set; }
        public string? Phone { get; set; }
        public string? GitHub { get; set; }
        public string? LinkedIn { get; set; }
        public int? EnglishLevel { get; set; }
        public int? ExperienceYears { get; set; }
        public string? Experience { get; set; }
        public virtual ICollection<SkillsTag> Skills { get; set; } = new List<SkillsTag>();
        public int? DesiredPositionId { get; set; }
        public Position? DesiredPosition { get; set; }
        public bool IsFilled { get; set; } = false;
        public bool IsAccepted { get; set; } = false;
        public int? Views { get; set; }
        public ICollection<JobSeekerVacancy> VacanciesApplied { get; set; }
        public string UserId { get; set; }
        public AppUser AppUser { get; set; }
        public int? CountryId { get; set; }
        public Country? Country { get; set; }
        public int? CityId { get; set; }
        public City? City { get; set; }
        public bool IsSubscribedToNotification { get; set; } = false;
    }
}