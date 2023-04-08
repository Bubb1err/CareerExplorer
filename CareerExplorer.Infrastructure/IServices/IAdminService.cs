using CareerExplorer.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerExplorer.Infrastructure.IServices
{
    public interface IAdminService
    {
        Task AcceptJobSeekerProfile(int id);
        Task AcceptRecruiterProfile(int id);
        bool IsRecuiterProfileFilled(Recruiter recruiter);
        bool IsJobSeekerProfileFilled(JobSeeker jobSeeker);
        Task AcceptVacancy(int id);
    }
}
