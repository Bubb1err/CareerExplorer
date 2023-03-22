using CareerExplorer.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CareerExplorer.Infrastructure.Data;
using CareerExplorer.Core.Entities;
using Microsoft.AspNetCore.Identity.UI.Services;
using CareerExplorer.Shared;
using CareerExplorer.Core.Interfaces;
using CareerExplorer.Infrastructure.Repository;
using CareerExplorer.Infrastructure.Services;

namespace CareerExplorer.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddMvc();
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
            builder.Services.AddAutoMapper(typeof(MappingConfig));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
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