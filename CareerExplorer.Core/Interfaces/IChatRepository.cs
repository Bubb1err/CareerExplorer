﻿using CareerExplorer.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerExplorer.Core.Interfaces
{
    public interface IChatRepository : IRepository<Chat>
    {
        IEnumerable<Chat> GetJobSeekerChats(AppUser appUser);
        IEnumerable<Chat> GetRecruiterChats(AppUser appUser);
    }
}
