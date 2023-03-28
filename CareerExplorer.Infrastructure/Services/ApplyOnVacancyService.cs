using CareerExplorer.Core.Entities;
using CareerExplorer.Core.Interfaces;
using CareerExplorer.Core.IServices;
using CareerExplorer.Infrastructure.Repository;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

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
            _jobSeekerRepository = _unitOfWork.GetJobSeekerRepository();
            _vacanciesRepository = _unitOfWork.GetVacanciesRepository();
            _jobSeekerVacancyRepository = _unitOfWork.GetJobSeekerVacancyRepository();
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
            var jobseekerVacancy = _jobSeekerVacancyRepository.GetFirstOrDefault(x => x.VacancyId == vacancyId && x.JobSeekerId == jobSeeker.Id);

            vacancyApplied.Applicants.Add(jobseekerVacancy);
            jobSeeker.VacanciesApplied.Add(jobseekerVacancy);
            await _unitOfWork.SaveAsync();

        }
    }
}
