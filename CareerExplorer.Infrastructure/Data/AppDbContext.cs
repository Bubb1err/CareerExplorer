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
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
        }
        public DbSet<Company> Companies { get; set; }
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
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            base.OnModelCreating(builder);

            var response = SeedData.GetCountriesFromExternalAPI();
            if (response != null && response.Error == false)
            {
                var countries = new List<Country>();
                var cities = new List<City>();
                int countryId = 1;
                int cityId = 1;
                foreach (var countryData in response.Data)
                {
                    var country = new Country { Id = countryId, Name = countryData.Country ?? "" };
                    countries.Add(country);

                    if (countryData.Cities != null)
                    {
                        foreach (var cityName in countryData.Cities)
                        {
                            var city = new City { Id = cityId, Name = cityName ?? "", CountryId = countryId };
                            cities.Add(city);
                            cityId++;
                        }
                    }
                    countryId++;
                }
                builder.Entity<Country>().HasData(countries);
                builder.Entity<City>().HasData(cities);
            }
        }
    }
}
