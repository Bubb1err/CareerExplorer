using CareerExplorer.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerExplorer.Core.Interfaces
{
    public interface IJobSeekerVacancyRepository : IRepository<JobSeekerVacancy>
    {
        IEnumerable<JobSeeker> GetApplicantsForVacancy(int vacancyId);
        void Update(JobSeekerVacancy entity);
    }
}
