using CareerExplorer.Core.Entities;
using CareerExplorer.Core.Interfaces;
using CareerExplorer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerExplorer.Infrastructure.Repository
{
    public class VacanciesRepository : Repository<Vacancy>, IVacanciesRepository
    {
        private readonly AppDbContext _context;
         
        public VacanciesRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public IEnumerable<Vacancy> GetAvailableVacancies()
        {
            var vacancies = dbSet.Where(x => x.IsAvailable == true); 
            return vacancies;
        }
        public IEnumerable<Vacancy> GetCreatedVacancies(string userId)
        {
            var recrId = _context.Recruiters.AsNoTracking().FirstOrDefault(x => x.UserId == userId).Id;
            var vacancies = dbSet.AsNoTracking().Where(x => x.CreatorId== recrId);
            return vacancies;
        }

        public void Update(Vacancy entity)
        {
            _context.Vacancies.Update(entity);
        }
    }
}
