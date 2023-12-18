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
		builder.Property(driver => driver.Name)
			   .HasColumnType("nvarchar(30)")
			   .IsRequired();

		builder.Property(driver => driver.PhoneNumber)
			   .HasColumnType("nchar(12)")
			   .IsRequired();

		builder.Property(driver => driver.Email)
			   .HasColumnType("nvarchar(63)")
			   .IsRequired();

		builder.HasIndex(driver => driver.PhoneNumber)
			   .HasDatabaseName("IX_Drivers_PhoneNumber")
			   .IsUnique();

		builder.Property(driver => driver.Password)
			   .HasColumnType("nvarchar(100)")
			   .IsRequired();

		builder.Property(driver => driver.BirthDate)
			   .HasColumnType("date")
			   .IsRequired();

		builder.Property(driver => driver.Salary)
			   .HasColumnType("int")
			   .IsRequired();

		builder.Property(driver => driver.DrivingLicense)
			   .HasColumnType("bigint")
			   .IsRequired();
	}
}
