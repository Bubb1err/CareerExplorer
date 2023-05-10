using CareerExplorer.Core.Entities;

namespace CareerExplorer.Infrastructure.IServices
{
    public interface IVacancyService
    {
        int[]? GetIdsFromString(string ids);
        Task CreateVacancy(string[] tags, int position, string currentRecruiterId, Vacancy vacancy);
        Task EditVacancy(string[] tags, Vacancy vacancy, int position);
    }
}
