using CareerExplorer.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CareerExplorer.Infrastructure.Data.EntityTypeConfiguration
{
    internal class RecruiterEntityConfig : IEntityTypeConfiguration<Recruiter>
    {
        public void Configure(EntityTypeBuilder<Recruiter> builder)
        {
            builder.HasOne(x => x.AppUser)
                .WithOne(x => x.RecruiterProfile)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(x => x.Vacancies)
                .WithOne(x => x.Creator)
                .HasForeignKey(x => x.CreatorId)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}