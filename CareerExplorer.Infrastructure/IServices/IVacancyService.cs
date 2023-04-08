using CareerExplorer.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerExplorer.Infrastructure.IServices
{
    public interface IVacancyService
    {
        int[]? GetIdsFromString(string ids);
        Task CreateVacancy(string selectedSkills, string position, string currentRecruiterId, Vacancy vacancy);
        Task EditVacancy(string selectedSkills, Vacancy vacancy, string position);
    }
}
