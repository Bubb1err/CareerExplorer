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
        int CountVacancies();
        IEnumerable<Vacancy> GetCreatedVacancies(string userId);
        IEnumerable<Vacancy> GetAvailablePaginatedVacancies(int pageSize, int pageNumber);
        void Update(Vacancy entity);
    }
}
