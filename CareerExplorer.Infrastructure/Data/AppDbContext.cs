using CareerExplorer.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace CareerExplorer.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<JobSeeker> JobSeekers { get; set; }
        public DbSet<Recruiter> Recruiters { get; set; }
        public DbSet<Vacancy> Vacancies { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<JobSeekerVacancy> JobSeekerVacancies { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<SkillsTag> SkillsTags { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Chat> Chat { get; set; }
        public DbSet<MeetingNotification> MeetingNotifications { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        protected override async void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            base.OnModelCreating(builder);
            await SeedData.SeedDataToDb(builder);
        }
    }
}
