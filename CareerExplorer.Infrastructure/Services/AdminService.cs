using CareerExplorer.Core.Entities;
using CareerExplorer.Core.Interfaces;
using CareerExplorer.Infrastructure.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            _vacanciesRepository = _unitOfWork.GetVacanciesRepository();
            _recruiterProfileRepository = _unitOfWork.GetRecruiterRepository();
            _jobSeekerRepository = _unitOfWork.GetJobSeekerRepository();
        }
        public async Task AcceptVacancy(int id)
        {
            var vacancy = _vacanciesRepository.GetFirstOrDefault(x => x.Id == id);
            vacancy.IsAccepted = true;
            await _unitOfWork.SaveAsync();
        }
        public async Task AcceptRecruiterProfile(int id)
        {
            var recruiter = _recruiterProfileRepository.GetFirstOrDefault(x => x.Id == id);
            recruiter.IsAccepted = true;
            await _unitOfWork.SaveAsync();
        }
        public async Task AcceptJobSeekerProfile(int id)
        {
            var jobSeeker = _jobSeekerRepository.GetFirstOrDefault(x => x.Id == id);
            jobSeeker.IsAccepted = true;
            await _unitOfWork.SaveAsync();
        }
        public bool IsJobSeekerProfileFilled (JobSeeker jobSeeker)
        {
            if(string.IsNullOrWhiteSpace(jobSeeker.Name)) return false;
            if (string.IsNullOrWhiteSpace(jobSeeker.Surname)) return false;
            if (string.IsNullOrWhiteSpace(jobSeeker.Experience) || jobSeeker.Experience.Length < 200) return false;
            if (string.IsNullOrWhiteSpace(jobSeeker.Phone)) return false;
            if(jobSeeker.Skills.Count == 0) return false;
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
