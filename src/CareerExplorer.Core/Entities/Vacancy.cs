
namespace CareerExplorer.Core.Entities
{
    public class Vacancy 
    {
        public int Id { get; set; }
        public bool IsAccepted { get; set; }= false;
        public int PositionId { get; set; }
        public virtual Position? Position { get; set; }
        public string Description { get; set; }
        public bool IsAvailable { get; set; }
        public virtual ICollection<SkillsTag> Requirements { get; set; } = new List<SkillsTag>();
        public virtual ICollection<JobSeekerVacancy> Applicants { get; set; } 
        public int CreatorId { get; set; }
        public virtual Recruiter Creator { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int? CountryId { get; set; }
        public Country? Country { get; set; }
        public int? CityId { get; set; }
        public City? City { get; set; }
        public int? Salary { get; set; }
        public int? WorkType { get; set; }
        public int? EnglishLevel { get; set; }
        public int? ExperienceYears { get; set; }
        public int Views { get; set; }
    }
}
