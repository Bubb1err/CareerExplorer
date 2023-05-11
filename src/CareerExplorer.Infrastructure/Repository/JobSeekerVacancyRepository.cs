using CareerExplorer.Core.Entities;
using CareerExplorer.Core.Interfaces;
using CareerExplorer.Infrastructure.Data;


namespace CareerExplorer.Infrastructure.Repository
{
    public class JobSeekerVacancyRepository : Repository<JobSeekerVacancy>, IJobSeekerVacancyRepository
    {
        private AppDbContext _context;
        public JobSeekerVacancyRepository(AppDbContext context) : base(context) 
        {
            _context = context;
        }
        public IEnumerable<JobSeeker> GetApplicantsForVacancy(int vacancyId)
        {
            return _context.JobSeekerVacancies
                .Where(x => x.VacancyId == vacancyId)
                .Select(x => x.JobSeeker);
        }

        public void Update(JobSeekerVacancy entity)
        {
            _context.JobSeekerVacancies.Update(entity);
        }
    }
}
