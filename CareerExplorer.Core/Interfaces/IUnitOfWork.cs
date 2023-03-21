using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerExplorer.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        //IJobSeekerProfileRepository JobSeekerProfile { get; }
        //IRecruiterProfileRepository RecruiterProfile { get; }
        IRepository<T> GetRepository<T>() where T : class;
        Task SaveAsync();
    }
}
