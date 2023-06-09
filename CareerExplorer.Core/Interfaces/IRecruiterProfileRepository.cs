﻿using CareerExplorer.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerExplorer.Core.Interfaces
{
    public interface IRecruiterProfileRepository : IRepository<Recruiter>
    {
        IEnumerable<Recruiter> GetRecruiterProfilesToAccept();
        void Update(Recruiter entity);
    }
}
