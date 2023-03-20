using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerExplorer.Core.Interfaces
{
    public interface IUnitOfWork
    {
        IJobSeekerProfileRepository JobSeekerProfile { get; }
        IRecruiterProfileRepository RecruiterProfile { get; }
        void Save();
    }
}
