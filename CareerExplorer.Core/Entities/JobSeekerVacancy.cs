
namespace CareerExplorer.Core.Entities
{
    public class JobSeekerVacancy
    {
        public int Id { get; set; }
        public int JobSeekerId { get; set; }
        public int VacancyId { get; set; }
        public byte[] Cv { get; set; }
        public bool IsApplied { get; set; } = false;
        public JobSeeker JobSeeker { get; set; }
        public Vacancy Vacancy { get; set; }
    }
}