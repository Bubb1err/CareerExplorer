using CareerExplorer.Core.Entities;
using CareerExplorer.Core.Interfaces;
using CareerExplorer.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerExplorer.Infrastructure.Repository
{
    public class AdminRepository : Repository<Admin>, IAdminRepository
    {
        private AppDbContext _context;
        public AdminRepository(AppDbContext context) : base(context) 
        {
            _context = context;
        }

        public void UpdateSkillTag(SkillsTag tag)
        {
            _context.SkillsTags.Update(tag);
        }
        public void UpdatePosition(Position position)
        {
            _context.Positions.Update(position);
        }
    }
}
