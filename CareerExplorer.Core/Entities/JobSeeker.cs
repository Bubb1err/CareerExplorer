using CareerExplorer.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerExplorer.Core.Entities
{
    public class JobSeeker 
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Phone { get; set; }
        public string? GitHub { get; set; }
        public string? Experience { get; set; }
        public int? Views { get; set; }
        public ICollection<CvPath> PathsToAppliedCvs { get; set; } = new List<CvPath>();
        public ICollection<JobSeekerVacancy> VacanciesApplied { get; set; }
        public string UserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
