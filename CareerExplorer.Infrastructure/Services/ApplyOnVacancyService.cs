﻿using CareerExplorer.Core.Entities;
using CareerExplorer.Core.Interfaces;
using CareerExplorer.Core.IServices;
using Microsoft.AspNetCore.Http;

namespace CareerExplorer.Infrastructure.Services
{
    public sealed class ApplyOnVacancyService : IApplyOnVacancyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJobSeekerProfileRepository _jobSeekerRepository;
        private readonly IVacanciesRepository _vacanciesRepository;
        private readonly IJobSeekerVacancyRepository _jobSeekerVacancyRepository;
        public ApplyOnVacancyService(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
            _jobSeekerRepository = (IJobSeekerProfileRepository)_unitOfWork.GetRepository<JobSeeker>();
            _vacanciesRepository = (IVacanciesRepository)_unitOfWork.GetRepository<Vacancy>();
            _jobSeekerVacancyRepository = (IJobSeekerVacancyRepository)_unitOfWork.GetRepository<JobSeekerVacancy>();
        }
        public async Task Apply(string currentLogedInUserId, int vacancyId, IFormFile file) 
        {
            var jobSeeker = _jobSeekerRepository.GetFirstOrDefault(x => x.UserId == currentLogedInUserId);
            if (jobSeeker == null)
            {
                throw new Exception();
            }
            var vacancyApplied = _vacanciesRepository.GetFirstOrDefault(x => x.Id == vacancyId);
            if (vacancyApplied == null) 
            {
                throw new Exception();
            }
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);

                // Upload the file if less than 2 MB
                if (memoryStream.Length < 2097152)
                {
                    var jobSeekerVacancy = new JobSeekerVacancy()
                    {
                        JobSeekerId = jobSeeker.Id,
                        JobSeeker = jobSeeker,
                        VacancyId = vacancyId,
                        Vacancy = vacancyApplied,
                        Cv = memoryStream.ToArray(),
                        IsApplied = true
                    };

                    await _jobSeekerVacancyRepository.AddAsync(jobSeekerVacancy);

                    await _unitOfWork.SaveAsync();
                }
                else throw new Exception();
            }
            var jobseekerVacancy = _jobSeekerVacancyRepository
                .GetFirstOrDefault(x => x.VacancyId == vacancyId && x.JobSeekerId == jobSeeker.Id);
            if(jobseekerVacancy == null)
                throw new NullReferenceException();
            vacancyApplied.Applicants.Add(jobseekerVacancy);
            jobSeeker.VacanciesApplied.Add(jobseekerVacancy);
            await _unitOfWork.SaveAsync();

        }
    }
}
