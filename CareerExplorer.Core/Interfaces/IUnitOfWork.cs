using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerExplorer.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IVacanciesRepository GetVacanciesRepository();
        IJobSeekerProfileRepository GetJobSeekerRepository();
        IRecruiterProfileRepository GetRecruiterRepository();
        ICvPathsRepository GetCvPathsRepository();
        IRepository<T> GetRepository<T>() where T : class;
        Task SaveAsync();
    }
}
