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
        public virtual ICollection<SkillsTag> Skills { get; set; } 
        public Position? DesiredPosition { get; set; }
        public bool IsFilled { get; set; } = false;
        public bool IsAccepted { get; set; } = false;
        public int? Views { get; set; }
        public ICollection<JobSeekerVacancy> VacanciesApplied { get; set; }
        public string UserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
