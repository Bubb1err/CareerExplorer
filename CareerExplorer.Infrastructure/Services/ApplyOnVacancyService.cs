using CareerExplorer.Core.Entities;
using CareerExplorer.Core.IServices;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerExplorer.Infrastructure.Services
{
    public sealed class ApplyOnVacancyService : IApplyOnVacancyService
    {
        public async Task<string> SaveCv(IFormFile cv, string destinationFolderPath, int jobSeekerId, int? vacancyId)
        {
            var fileName = Guid.NewGuid().ToString() + '-' + jobSeekerId + '-' + vacancyId + Path.GetExtension(cv.FileName);
            var fullPath = Path.Combine(destinationFolderPath, fileName);
            if (!Directory.Exists(destinationFolderPath))
            {
                Directory.CreateDirectory(destinationFolderPath);
            }
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await cv.CopyToAsync(stream);
            }
            return fileName;
        }
        public int GetJobSeekerIdFromFilePath(string path)
        {
            var parts = path.Split('-');
            int.TryParse(parts[1], out int jobSeekerId);
            return jobSeekerId;

        }
        public CvPath CreateCvPath(string path, int jobSeekerId, JobSeeker jobSeeker)
        {
            return new CvPath()
            {
                Path = path,
                JobSeekerId = jobSeekerId,
                JobSeeker = jobSeeker
            };
        }
        public JobSeekerVacancy CreateJobSeekerVacancy(int jobSeekerId, JobSeeker jobSeeker, int appliedVacancyId, Vacancy appliedVacancy)
        {
            return new JobSeekerVacancy()
            {
                JobSeekerId = jobSeekerId,
                JobSeeker = jobSeeker,
                Vacancy = appliedVacancy,
                VacancyId = appliedVacancyId,
                IsApplied = true
            };
        }
    }
}
