using CareerExplorer.Core.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerExplorer.Infrastructure.Data.EntityTypeConfiguration
{
    internal class AdminEntityConfig : IEntityTypeConfiguration<Admin>
    {
        public void Configure(EntityTypeBuilder<Admin> builder)
        {
            builder.HasMany(x => x.Tags).WithOne(x => x.Admin).HasForeignKey(x => x.AdminId);
            builder.HasMany(x => x.Positions).WithOne(x => x.Admin).HasForeignKey(x => x.AdminId);

        }
    }
}
