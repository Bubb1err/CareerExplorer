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
        Task Apply(string currentLogedInUserId, int vacancyId, IFormFile file);
    }
}
