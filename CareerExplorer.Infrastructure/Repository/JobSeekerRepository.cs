﻿using CareerExplorer.Core.Entities;
using CareerExplorer.Core.Interfaces;
using CareerExplorer.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerExplorer.Infrastructure.Repository
{
    public class JobSeekerRepository : Repository<JobSeeker>, IJobSeekerProfileRepository
    {
        private AppDbContext _context;
        public JobSeekerRepository(AppDbContext context) : base(context) 
        {
            _context = context;
        }

    }
}
