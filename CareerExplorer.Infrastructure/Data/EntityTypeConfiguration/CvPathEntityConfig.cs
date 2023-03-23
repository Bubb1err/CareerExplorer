using CareerExplorer.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CareerExplorer.Infrastructure.Data.EntityTypeConfiguration
{
    public class CvPathEntityConfig : IEntityTypeConfiguration<CvPath>
    {
        public void Configure(EntityTypeBuilder<CvPath> builder)
        {
            builder.HasOne(x => x.JobSeeker)
                .WithMany(x => x.PathsToAppliedCvs)
                .HasForeignKey(x => x.JobSeekerId);
        }
    }
}