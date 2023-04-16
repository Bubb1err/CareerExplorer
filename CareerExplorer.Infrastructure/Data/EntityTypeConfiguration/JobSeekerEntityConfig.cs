using CareerExplorer.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CareerExplorer.Infrastructure.Data.EntityTypeConfiguration
{
    internal class JobSeekerEntityConfig : IEntityTypeConfiguration<JobSeeker>
    {
        public void Configure(EntityTypeBuilder<JobSeeker> builder)
        {
            builder.HasOne(x => x.AppUser)
                .WithOne(x => x.JobSeekerProfile)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(x => x.VacanciesApplied)
                .WithOne(x => x.JobSeeker)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.City);
            builder.HasOne(x => x.Country);

        }
    }
}
