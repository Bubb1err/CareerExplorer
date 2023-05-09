
namespace CareerExplorer.Core.Entities
{
    public class SkillsTag
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public virtual ICollection<JobSeeker> JobSeekers { get; set; } = new List<JobSeeker>();
        public virtual ICollection<Vacancy> Vacancies { get; set; } = new List<Vacancy>();

    }
}
