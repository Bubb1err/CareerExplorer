using CareerExplorer.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CareerExplorer.Infrastructure.Data;
using CareerExplorer.Core.Entities;
using Microsoft.AspNetCore.Identity.UI.Services;
using CareerExplorer.Shared;
using Microsoft.AspNetCore.Localization;
using CareerExplorer.Core.Interfaces;
using CareerExplorer.Infrastructure.Repository;
using CareerExplorer.Infrastructure.Services;
using CareerExplorer.Core.IServices;
using Microsoft.Extensions.FileSystemGlobbing.Internal.Patterns;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;

namespace CareerExplorer.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddMvc()
                .AddDataAnnotationsLocalization()
                .AddViewLocalization();
            builder.Services.AddRazorPages();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext(connectionString);

            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<AppDbContext>();

            builder.Services.AddSingleton<IEmailSender, EmailSender>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IJobSeekerProfileRepository, JobSeekerRepository>();
            builder.Services.AddScoped<IRecruiterProfileRepository, RecruiterProfileRepository>();
            builder.Services.AddScoped<IVacanciesRepository, VacanciesRepository>();         
            builder.Services.AddScoped<IApplyOnVacancyService, ApplyOnVacancyService>();
            builder.Services.AddScoped<IJobSeekerVacancyRepository, JobSeekerVacancyRepository>();
            builder.Services.AddScoped<IRepository<AppUser>, Repository<AppUser>>();
            builder.Services.AddAutoMapper(typeof(MappingConfig));

            builder.Services.AddLocalization(options => options.ResourcesPath = "Recources");
            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                var supported = new[]
                {
                    new CultureInfo("en"),
                    new CultureInfo("uk")
                };
                options.DefaultRequestCulture = new RequestCulture("en");
                options.SupportedCultures = supported;
                options.SupportedUICultures= supported;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRequestLocalization();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
                
            app.UseAuthorization();
            app.MapRazorPages();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}