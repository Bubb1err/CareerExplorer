using CareerExplorer.Core.Entities;
using CareerExplorer.Core.Interfaces;
using CareerExplorer.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerExplorer.Infrastructure.Repository
{
    public class CvPathsRepository : Repository<CvPath>, ICvPathsRepository
    {
        private AppDbContext _context;
        public CvPathsRepository(AppDbContext context) : base(context) 
        {
            _context = context;
        }
        

        public void Update(CvPath path)
        {
            _context.CvPaths.Update(path);
        }
    }
}
