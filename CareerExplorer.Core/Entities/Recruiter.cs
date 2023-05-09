
namespace CareerExplorer.Core.Entities
{
    public class Recruiter 
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string CompanyDescription { get; set; } = string.Empty;
        public bool IsFilled { get; set; } = false;
        public bool IsAccepted { get; set; } = false;
        public string UserId { get; set; }
        public AppUser AppUser { get; set; }
        public List<Vacancy> Vacancies { get; set; } = new List<Vacancy>();
    }
}
