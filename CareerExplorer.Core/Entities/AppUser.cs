using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerExplorer.Core.Entities
{
    public class AppUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int? JobSeekerProfileId { get; set; }
        public JobSeeker JobSeekerProfile { get; set; }
        public int? RecruiterProfileId { get; set; }
        public Recruiter RecruiterProfile { get; set; }
    }
}
