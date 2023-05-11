using CareerExplorer.Core.Entities;
using CareerExplorer.Core.Interfaces;
using CareerExplorer.Infrastructure.Data;

namespace CareerExplorer.Infrastructure.Repository
{
    public class AdminRepository : Repository<Admin>, IAdminRepository
    {
        private readonly AppDbContext _context;
        public AdminRepository(AppDbContext context) : base(context) 
        {
            _context = context;
        }

        public void UpdateSkillTag(SkillsTag tag)
        {
            _context.SkillsTags.Update(tag);
        }
        public void UpdatePosition(Position position)
        {
            _context.Positions.Update(position);
        }
    }
}
