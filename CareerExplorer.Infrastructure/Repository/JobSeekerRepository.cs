using CareerExplorer.Core.Entities;
using CareerExplorer.Core.Interfaces;
using CareerExplorer.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return _context.JobSeekers.Include(x => x.Skills).FirstOrDefault(x => x.UserId == userId);
        }
        public void Update(JobSeeker jobSeeker)
        {
            _context.JobSeekers.Update(jobSeeker);
        }
    }
}
