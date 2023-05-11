using CareerExplorer.Core.Entities;

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
