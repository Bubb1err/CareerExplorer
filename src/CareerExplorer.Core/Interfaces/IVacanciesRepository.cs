using CareerExplorer.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CareerExplorer.Core.Interfaces
{
    public interface IVacanciesRepository : IRepository<Vacancy>
    {
        IEnumerable<Vacancy> GetCreatedVacancies(string userId);
        void Update(Vacancy entity);
        Task<Vacancy> GetVacancyAsync(int id);
        IEnumerable<Vacancy> GetVacanciesToAccept();
        IEnumerable<Vacancy> GetAvailablePaginatedAndFilteredVacancies
            (int pageSize, int pageNumber, out int countVacancies, Expression<Func<Vacancy, bool>> filter);
        int CountApplicantsOnVacancy(int vacancyId);
    }
}
