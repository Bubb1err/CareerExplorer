using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerExplorer.Core.Entities
{
    public class JobSeekerVacancy
    {
        public int Id { get; set; }
        public int JobSeekerId { get; set; }
        public int VacancyId { get; set; }
        public bool IsApplied { get; set; } = false;
        public JobSeeker JobSeeker { get; set; }
        public Vacancy Vacancy { get; set; }
    }
}
