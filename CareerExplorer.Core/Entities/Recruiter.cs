using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerExplorer.Core.Entities
{
    public class Recruiter
    {
        public int Id { get; set; }
        //[ForeignKey("Company")]
        //public int CompanyId { get; set; }
        //public Company Company { get; set; }
        [ForeignKey("AppUser")]
        public string UserId { get; set; }
        public AppUser AppUser { get; set; }
        public List<Vacancy> Vacancies { get; set; } = new List<Vacancy>();
    }
}
