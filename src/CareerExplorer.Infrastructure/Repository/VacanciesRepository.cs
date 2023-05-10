using CareerExplorer.Core.Entities;
using CareerExplorer.Core.Interfaces;
using CareerExplorer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Linq;
using System.Linq.Expressions;

namespace CareerExplorer.Infrastructure.Repository
{
    public class VacanciesRepository : Repository<Vacancy>, IVacanciesRepository
    {
        private readonly AppDbContext _context;
         
        public VacanciesRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public IEnumerable<Vacancy> GetAvailablePaginatedAndFilteredVacancies(int pageSize, int pageNumber, out int countVacancies,
            Expression<Func<Vacancy, bool>> filter)
        {
            IQueryable<Vacancy> vacancies = dbSet;
            vacancies = vacancies.AsNoTracking().Where(x => x.IsAvailable == true && x.IsAccepted == true)
                .Include(x => x.Creator)
                    .ThenInclude(x => x.AppUser)
                .Include(x => x.Requirements)
                .Include(x => x.Position);

            if (filter != null)
            {
                vacancies = vacancies.Where(filter);
            }

            countVacancies = vacancies.Count();
            vacancies = (IQueryable<Vacancy>)Paginate(vacancies, pageSize, pageNumber);
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
                .Include(x => x.Requirements)
                .Include(x => x.City)
                .Include(x => x.Country)
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
        public int CountApplicantsOnVacancy (int vacancyId)
        {
            int vacanciesCount = _context.JobSeekerVacancies.Where(x  => x.VacancyId == vacancyId).Count();
            return vacanciesCount;
        }
        public void Update(Vacancy entity)
        {
            _context.Vacancies.Update(entity);
        }
    }
}
