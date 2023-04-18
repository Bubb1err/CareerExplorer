using CareerExplorer.Core.Entities;
using CareerExplorer.Core.Interfaces;
using CareerExplorer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Linq;

namespace CareerExplorer.Infrastructure.Repository
{
    public class VacanciesRepository : Repository<Vacancy>, IVacanciesRepository
    {
        private readonly AppDbContext _context;
         
        public VacanciesRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public IEnumerable<Vacancy> GetAvailablePaginatedAndFilteredVacancies(int pageSize, int pageNumber, out int countVacancies, int[]? tagsIds = null, int[] types = null)
        {
            IQueryable<Vacancy> vacancies = dbSet;
            if(tagsIds == null && types == null)
            {
                vacancies = vacancies.AsNoTracking().Where(x => x.IsAvailable == true && x.IsAccepted == true)
                .Include(x => x.Creator)
                    .ThenInclude(x => x.AppUser)
                .Include(x => x.Requirements)
                .Include(x => x.Position);
                countVacancies= vacancies.Count();
            }
            else if(tagsIds != null && types == null)
            {
                vacancies = vacancies.AsNoTracking().Where(x => x.IsAvailable 
                && x.IsAccepted == true && x.Requirements.Any(x => tagsIds.Contains(x.Id)))
                .Include(x => x.Creator)
                    .ThenInclude (x => x.AppUser)
                .Include(x => x.Requirements)
                .Include(x => x.Position);
                countVacancies = vacancies.Count();
            }
            else if(tagsIds == null && types != null )
            {
                vacancies = vacancies.AsNoTracking().Where(x => x.IsAvailable && x.IsAccepted == true 
                && x.WorkType != null && types.Contains((int)x.WorkType))
                .Include(x => x.Creator)
                    .ThenInclude(x => x.AppUser)
                .Include(x => x.Requirements)
                .Include(x => x.Position);
                countVacancies = vacancies.Count();
            }
            else
            {
                vacancies = vacancies.AsNoTracking().Where(x => x.IsAvailable && x.IsAccepted == true
                && x.WorkType != null && types.Contains((int)x.WorkType) 
                && x.Requirements.Any(x => tagsIds.Contains(x.Id)))
                                .Include(x => x.Creator)
                                    .ThenInclude(x => x.AppUser)
                                .Include(x => x.Requirements)
                                .Include(x => x.Position);
                countVacancies = vacancies.Count();
            }
            if (pageSize > 0)
            {
                if (pageSize > 100)
                {
                    pageSize = 100;
                }
                vacancies = vacancies.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
            }  
            return vacancies;
        }
        public IEnumerable<Vacancy> GetCreatedVacancies(string userId)
        {
            if(userId== null)
                throw new ArgumentNullException("userId");
            var recruiter = _context.Recruiters.AsNoTracking().FirstOrDefault(x => x.UserId == userId);
            if (recruiter == null)
                throw new Exception();
            var recuiterId = recruiter.Id;
            var vacancies = dbSet.AsNoTracking().Where(x => x.CreatorId== recuiterId).Include(x => x.Position);
            return vacancies;
        }
        public async Task<Vacancy> GetVacancyAsync(int id)
        {
            if (id == 0) throw new ArgumentException();
            var vacancy =  await _context.Vacancies
                .Include(x => x.Creator)
                .ThenInclude(x => x.AppUser)
                .Include(x => x.Position)
                .FirstOrDefaultAsync(x => x.Id == id);
            if(vacancy == null)
                throw new NullReferenceException();
            return vacancy;
        }
        public IEnumerable<Vacancy> GetVacanciesToAccept()
        {
           var vacancies =  _context.Vacancies
                .Include(x => x.Creator)
                .ThenInclude(x => x.AppUser)
                .Include(x => x.Position)
                .Where(x => !x.IsAccepted);
            return vacancies;
        }
        public void Update(Vacancy entity)
        {
            _context.Vacancies.Update(entity);
        }
    }
}
