
namespace CareerExplorer.Core.Entities
{
    public class City
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? CountryId { get; set; }
        public Country? Country { get; set; }
        public virtual ICollection<JobSeeker>? JobSeekers { get; set; }
        public virtual ICollection<Vacancy>? Vacancies { get; set; }
    }
}
