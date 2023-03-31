using CareerExplorer.Core.Entities;
using CareerExplorer.Core.Interfaces;
using CareerExplorer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CareerExplorer.Infrastructure.Repository
{
    public class VacanciesRepository : Repository<Vacancy>, IVacanciesRepository
    {
        private readonly AppDbContext _context;
         
        public VacanciesRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public int CountVacancies()
        {
            return dbSet.AsNoTracking().Count(x => x.IsAvailable == true);
        }
        public IEnumerable<Vacancy> GetAvailablePaginatedVacancies(int pageSize, int pageNumber)
        {
            IQueryable<Vacancy> vacancies = dbSet;
            vacancies = vacancies.AsNoTracking().Where(x => x.IsAvailable == true)
                .Include(x => x.Creator);
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
            var vacancies = dbSet.AsNoTracking().Where(x => x.CreatorId== recuiterId);
            return vacancies;
        }
        public void Update(Vacancy entity)
        {
            _context.Vacancies.Update(entity);
        }
    }
}
