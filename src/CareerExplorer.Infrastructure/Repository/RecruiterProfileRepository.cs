using CareerExplorer.Core.Entities;
using CareerExplorer.Core.Interfaces;
using CareerExplorer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CareerExplorer.Infrastructure.Repository
{
    public class RecruiterProfileRepository : Repository<Recruiter>, IRecruiterProfileRepository
    {
        private AppDbContext _context;
        public RecruiterProfileRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public IEnumerable<Recruiter> GetRecruiterProfilesToAccept()
        {
            return _context.Recruiters.AsNoTracking().Where(x => x.IsFilled && !x.IsAccepted);
        }
        public void Update(Recruiter entity)
        {
            _context.Recruiters.Update(entity);
        }
    }
}
