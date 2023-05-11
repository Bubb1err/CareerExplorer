using CareerExplorer.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CareerExplorer.Infrastructure.Data.EntityTypeConfiguration
{
    internal class JobSeekerVacancyEntityConfig : IEntityTypeConfiguration<JobSeekerVacancy>
    {
        public void Configure(EntityTypeBuilder<JobSeekerVacancy> builder)
        {
            builder.HasKey(x => new {x.VacancyId, x.JobSeekerId});

            builder.HasOne(x => x.JobSeeker)
                .WithMany(x => x.VacanciesApplied)
                .HasForeignKey(x => x.JobSeekerId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Vacancy)
                .WithMany(x => x.Applicants)
                .HasForeignKey(x => x.VacancyId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
