using CareerExplorer.Core.Entities;
using CareerExplorer.Core.Interfaces;
using CareerExplorer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CareerExplorer.Infrastructure.Repository
{
    public class ChatRepository :Repository<Chat>, IChatRepository
    {
        private readonly AppDbContext _context;
        public ChatRepository(AppDbContext db) : base(db)
        {
            _context = db;
        }

        public IEnumerable<Chat> GetJobSeekerChats(AppUser appUser) 
        {
           return _context.Chat.Where(x => x.Users.Contains(appUser))
                .Include(x => x.Users)
                    .ThenInclude(x => x.RecruiterProfile)
                .Include(x => x.Messages);
        }
        public IEnumerable<Chat> GetRecruiterChats(AppUser appUser)
        {
            return _context.Chat.Where(x => x.Users.Contains(appUser))
                 .Include(x => x.Users)
                     .ThenInclude(x => x.JobSeekerProfile)
                 .Include(x => x.Messages);
        }
    }
}
