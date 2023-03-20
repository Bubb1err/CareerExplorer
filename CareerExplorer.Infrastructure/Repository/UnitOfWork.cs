using CareerExplorer.Core.Entities;
using CareerExplorer.Core.Interfaces;
using CareerExplorer.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerExplorer.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private AppDbContext _db;

        public UnitOfWork(AppDbContext db)
        {
            _db = db;
            JobSeekerProfile = new JobSeekerRepository(db);
            RecruiterProfile = new RecruiterProfileRepository(db);
        }
        public IJobSeekerProfileRepository JobSeekerProfile { get; private set; }
        public IRecruiterProfileRepository RecruiterProfile { get; private set; }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
