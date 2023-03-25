using CareerExplorer.Core.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerExplorer.Core.IServices
{
    public interface IApplyOnVacancyService
    {
        Task<string> SaveCv(IFormFile cv, string destinationFolderPath, int jobSeekerId, int? vacancyId);
        JobSeekerVacancy CreateJobSeekerVacancy(int jobSeekerId, JobSeeker jobSeeker, int appliedVacancyId, Vacancy appliedVacancy, string cvPath);
    }
}
