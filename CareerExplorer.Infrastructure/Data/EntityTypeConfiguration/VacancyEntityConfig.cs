using CareerExplorer.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerExplorer.Infrastructure.Data.EntityTypeConfiguration
{
    internal class VacancyEntityConfig : IEntityTypeConfiguration<Vacancy>
    {
        public void Configure(EntityTypeBuilder<Vacancy> builder)
        {
            builder.HasOne(x => x.Creator);
            builder.HasOne(x => x.Position);
            builder.HasOne(x => x.WorkType).WithOne(x => x.Vacancy).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
