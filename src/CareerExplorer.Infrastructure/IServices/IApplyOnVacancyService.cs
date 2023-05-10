using Microsoft.AspNetCore.Http;

namespace CareerExplorer.Core.IServices
{
    public interface IApplyOnVacancyService
    {
        Task Apply(string currentLogedInUserId, int vacancyId, IFormFile file);
    }
}
