using CareerExplorer.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerExplorer.Core.Interfaces
{
    public interface IVacanciesRepository : IRepository<Vacancy>
    {
        IEnumerable<Vacancy> GetCreatedVacancies(string userId);
        IEnumerable<Vacancy> GetAvailablePaginatedAndFilteredVacancies(int pageSize, int pageNumber, out int countVacancies, int[]? tagsIds = null);
        void Update(Vacancy entity);
        Task<Vacancy> GetVacancyAsync(int id);
        IEnumerable<Vacancy> GetVacanciesToAccept();
    }
}
