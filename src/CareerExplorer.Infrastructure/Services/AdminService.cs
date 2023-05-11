using CareerExplorer.Core.Entities;
using CareerExplorer.Core.Interfaces;
using CareerExplorer.Infrastructure.IServices;

namespace CareerExplorer.Infrastructure.Services
{
    public sealed class AdminService : IAdminService
    {
        private readonly IVacanciesRepository _vacanciesRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRecruiterProfileRepository _recruiterProfileRepository;
        private readonly IJobSeekerProfileRepository _jobSeekerRepository;

        public AdminService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _vacanciesRepository = (IVacanciesRepository)_unitOfWork.GetRepository<Vacancy>();
            _recruiterProfileRepository = (IRecruiterProfileRepository)_unitOfWork.GetRepository<Recruiter>();
            _jobSeekerRepository = (IJobSeekerProfileRepository)_unitOfWork.GetRepository<JobSeeker>();
        }
        public async Task AcceptVacancy(int id)
        {
            var vacancy = _vacanciesRepository.GetFirstOrDefault(x => x.Id == id);
            if(vacancy == null)
            {
                throw new NullReferenceException();
            }
            vacancy.IsAccepted = true;
            await _unitOfWork.SaveAsync();
        }
        public async Task AcceptRecruiterProfile(int id)
        {
            var recruiter = _recruiterProfileRepository.GetFirstOrDefault(x => x.Id == id);
            if(recruiter == null)
            {
                throw new NullReferenceException();
            }
            recruiter.IsAccepted = true;
            await _unitOfWork.SaveAsync();
        }
        public async Task AcceptJobSeekerProfile(int id)
        {
            var jobSeeker = _jobSeekerRepository.GetFirstOrDefault(x => x.Id == id);
            if(jobSeeker == null)
            {
                throw new NullReferenceException();
            }
            jobSeeker.IsAccepted = true;
            await _unitOfWork.SaveAsync();
        }
        public bool IsJobSeekerProfileFilled (JobSeeker jobSeeker)
        {
            if(string.IsNullOrWhiteSpace(jobSeeker.Name)) return false;
            if (string.IsNullOrWhiteSpace(jobSeeker.Surname)) return false;
            if (string.IsNullOrWhiteSpace(jobSeeker.Experience) || jobSeeker.Experience.Length < 200) return false;
            if (string.IsNullOrWhiteSpace(jobSeeker.Phone)) return false;
            if (jobSeeker.EnglishLevel == null) return false;
            if(jobSeeker.ExperienceYears== null) return false;
            if(jobSeeker.DesiredPosition == null) return false;
            if(jobSeeker.Skills.Count == 0) return false;
            if(jobSeeker.Country== null) return false;
            if(jobSeeker.City== null) return false;
            return true;
        }
        public bool IsRecuiterProfileFilled(Recruiter recruiter)
        {
            if (string.IsNullOrWhiteSpace(recruiter.Name)) return false;
            if (string.IsNullOrWhiteSpace(recruiter.Surname)) return false;
            if (string.IsNullOrWhiteSpace(recruiter.Company)) return false;
            if (string.IsNullOrWhiteSpace(recruiter.CompanyDescription) || recruiter.CompanyDescription.Length < 200) return false;
            return true;
        }
    }
}
