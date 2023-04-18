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
            builder.HasMany(x => x.Applicants)
                .WithOne(x => x.Vacancy)
                .HasForeignKey(x => x.VacancyId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Position)
                .WithMany(x => x.Vacancies)
                .HasForeignKey(x=>x.PositionId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Country)
                .WithMany(x => x.Vacancies)
                .HasForeignKey(x => x.CountryId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.City)
                .WithMany(x => x.Vacancies)
                .HasForeignKey(x => x.CityId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
