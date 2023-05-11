
namespace CareerExplorer.Core.Entities
{
    public class Position
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Vacancy>? Vacancies { get; set; }
        public virtual ICollection<JobSeeker>? JobSeekers { get; set; }
    }
}