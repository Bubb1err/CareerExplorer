using CareerExplorer.Core.Entities;
using CareerExplorer.Core.Interfaces;
using CareerExplorer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CareerExplorer.Infrastructure.Repository
{
    public class JobSeekerRepository : Repository<JobSeeker>, IJobSeekerProfileRepository
    {
        private AppDbContext _context;
        public JobSeekerRepository(AppDbContext context) : base(context) 
        {
            _context = context;
        }

        public JobSeeker GetJobSeeker(string userId)
        {
            var jobSeeker = _context.JobSeekers.Include(x => x.Skills).FirstOrDefault(x => x.UserId == userId);
            if (jobSeeker == null) throw new NullReferenceException();
            return jobSeeker;
        }
        public IEnumerable<JobSeeker> GetJobSeekersToAccept()
        {
            var jobSeekers = _context.JobSeekers
                .Where(x => x.IsFilled && !x.IsAccepted)
                .Include(x => x.AppUser)
                .AsNoTracking();
            return jobSeekers;
        }
        public void Update(JobSeeker jobSeeker)
        {
            _context.JobSeekers.Update(jobSeeker);
        }
    }
}
