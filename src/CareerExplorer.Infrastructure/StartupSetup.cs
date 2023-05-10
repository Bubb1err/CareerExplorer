using CareerExplorer.Core.Entities;
using CareerExplorer.Core.Interfaces;
using CareerExplorer.Core.IServices;
using CareerExplorer.Infrastructure.Data;
using CareerExplorer.Infrastructure.IServices;
using CareerExplorer.Infrastructure.Repository;
using CareerExplorer.Infrastructure.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CareerExplorer.Infrastructure
{
    public static class StartupSetup
    {
        public static void AddDbContext(this IServiceCollection services, string connectionString) =>
            services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));
        public static void RegisterRepositories(this IServiceCollection services)
        {         
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IRepository<JobSeeker>, JobSeekerRepository>();
            services.AddScoped<IRepository<Recruiter>, RecruiterProfileRepository>();
            services.AddScoped<IRepository<Vacancy>, VacanciesRepository>();                 
            services.AddScoped<IRepository<JobSeekerVacancy>, JobSeekerVacancyRepository>();
            services.AddScoped<IRepository<AppUser>, Repository<AppUser>>();
            services.AddScoped<IRepository<Admin>, AdminRepository>();
            services.AddScoped<IRepository<SkillsTag>, Repository<SkillsTag>>();
            services.AddScoped<IRepository<Country>, Repository<Country>>();
            services.AddScoped<IRepository<Position>, Repository<Position>>();           
            services.AddScoped<IRepository<Message>, Repository<Message>>();
            services.AddScoped<IRepository<Chat>, ChatRepository>();
            services.AddScoped<IRepository<Country>, CountryRepository>();
            services.AddScoped<IRepository<City>, Repository<City>>();
            services.AddScoped<IRepository<MeetingNotification>, Repository<MeetingNotification>>();
            services.AddScoped<IRepository<RefreshToken>, Repository<RefreshToken>>();
        }
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IVacancyService, VacancyService>();
            services.AddScoped<IApplyOnVacancyService, ApplyOnVacancyService>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IRecommendVacanciesByEmailService, RecommendVacanciesByEmailService>();
        }

    }

}
