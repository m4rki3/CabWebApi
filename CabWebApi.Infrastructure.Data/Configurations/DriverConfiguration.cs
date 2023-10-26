using CabWebApi.Domain.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CabWebApi.Infrastructure.Data.Configurations;
public class DriverConfiguration : IEntityTypeConfiguration<Driver>
{
    public void Configure(EntityTypeBuilder<Driver> builder)
    {
        builder.HasKey(driver => driver.Id)
               .HasName("PK_Drivers_Id");
               //.IsClustered();

        builder.Property(driver => driver.Name)
               .HasColumnType("nvarchar(20)")
               .IsRequired();

        builder.Property(driver => driver.Salary)
               .HasColumnType("int")
               .IsRequired();

        builder.Property(driver => driver.Experience)
               .HasColumnType("tinyint")
               .IsRequired();
    }
}
