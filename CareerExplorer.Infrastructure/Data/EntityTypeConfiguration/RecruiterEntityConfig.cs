using CareerExplorer.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

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
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
