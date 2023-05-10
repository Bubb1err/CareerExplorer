using CareerExplorer.Core.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerExplorer.Core.Interfaces
{
    public interface IAdminRepository : IRepository<Admin>
    {
        void UpdateSkillTag(SkillsTag tag);
        void UpdatePosition(Position position);
    }
}
