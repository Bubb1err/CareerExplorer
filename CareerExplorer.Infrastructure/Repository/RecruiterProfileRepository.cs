using CareerExplorer.Core.Entities;
using CareerExplorer.Core.Interfaces;
using CareerExplorer.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerExplorer.Infrastructure.Repository
{
    public class RecruiterProfileRepository : Repository<Recruiter>, IRecruiterProfileRepository
    {
        private AppDbContext _context;
        public RecruiterProfileRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Recruiter entity)
        {
            _context.Recruiters.Update(entity);
        }
    }
}
