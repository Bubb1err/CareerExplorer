using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerExplorer.Core.Entities
{
    public class Country
    {
        public int Id { get; set; }
        public string? Name { get; set; } 
        public virtual ICollection<JobSeeker>? JobSeekers { get; set; }
        public ICollection<City>? Cities { get; set; }
        public virtual ICollection<Vacancy>? Vacancies { get; set; }
    }
}
