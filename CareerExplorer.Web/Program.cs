using CareerExplorer.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CareerExplorer.Infrastructure.Data;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using CareerExplorer.Web.Hubs;
using Hangfire;
using Microsoft.AspNetCore.SignalR;
using System.Drawing.Text;
using CareerExplorer.Infrastructure.IServices;

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
            builder.Services.AddHangfire(x => x.UseSqlServerStorage(connectionString));
            builder.Services.AddHangfireServer();

            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<AppDbContext>();
            builder.Services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
                })
                .AddLinkedIn(options =>
                {
                    options.ClientId = builder.Configuration["Authentication:LinkedIn:ClientId"];
                    options.ClientSecret = builder.Configuration["Authentication:LinkedIn:ClientSecret"];
                });
                
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            });

            builder.Services.RegisterRepositories();
            builder.Services.RegisterServices();

            builder.Services.AddAutoMapper(typeof(MappingConfig));
            builder.Services.AddSignalR();

            builder.Services.AddLocalization(options => options.ResourcesPath = "Recources");
            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                var supported = new[]
                {
                    new CultureInfo("uk"),
                    new CultureInfo("en")
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
            app.UseHangfireDashboard();

            using (var serviceScope = app.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;
                var recommendVacanciesService = services.GetService<IRecommendVacanciesByEmailService>();
                RecurringJob.AddOrUpdate(() =>
                recommendVacanciesService.SendVacanciesToUsersByEmail(TimeSpan.FromDays(1)), Cron.Daily);

            }
            app.MapRazorPages();
            app.MapHub<ChatHub>("/chatHub");
            app.MapHub<NotificationHub>("/notificationHub");
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Vacancy}/{action=GetAll}/{id?}");

            app.Run();
        }
    }
}