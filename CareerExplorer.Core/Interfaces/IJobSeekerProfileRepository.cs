﻿using CareerExplorer.Core.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerExplorer.Core.Interfaces
{
    public interface IJobSeekerProfileRepository : IRepository<JobSeeker>
    {
        IEnumerable<JobSeeker> GetJobSeekersToAccept();
        JobSeeker GetJobSeeker(string userId);
        void Update(JobSeeker jobSeeker);
    }
}
