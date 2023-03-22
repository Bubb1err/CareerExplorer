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
        public async Task<string> SaveCv(IFormFile cv, string destinationFolderPath, int? jobSeekerId, int? vacancyId)
        {
            var fileName = Guid.NewGuid().ToString() + '-' + jobSeekerId + '-' + vacancyId + Path.GetExtension(cv.FileName);
            var fullPath = Path.Combine(destinationFolderPath, fileName);
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await cv.CopyToAsync(stream);
            }
            return fileName;
        }
    }
}
