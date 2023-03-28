﻿using CareerExplorer.Core.Interfaces;
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
        public string Description { get; set; }
        public bool IsAvailable { get; set; }
        public ICollection<JobSeekerVacancy> Applicants { get; set; } 
        public int CreatorId { get; set; }
        public virtual Recruiter Creator { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
