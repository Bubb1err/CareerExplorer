using CareerExplorer.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerExplorer.Core.Entities
{
    public class Vacancy 
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsAccepted { get; set; }= false;
        //public Position Position { get; set; }
        //public WorkType WorkType { get; set; }
        public ICollection<Country> Countries { get; set; }
        public string Description { get; set; }
        public bool IsAvailable { get; set; }
        public ICollection<SkillsTag> Requirements { get; set; } = new List<SkillsTag>();
        public ICollection<JobSeekerVacancy> Applicants { get; set; } 
        public int CreatorId { get; set; }
        public virtual Recruiter Creator { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
