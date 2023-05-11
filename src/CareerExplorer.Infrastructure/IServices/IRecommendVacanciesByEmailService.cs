
namespace CareerExplorer.Infrastructure.IServices
{
    public interface IRecommendVacanciesByEmailService
    {
        Task SendVacanciesToUsersByEmail(TimeSpan checkingPeriod);
    }
}
