﻿using Microsoft.EntityFrameworkCore;
using CabWebApi.Domain.Core;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CabWebApi.Infrastructure.Data.Configurations;
public class CarConfiguration : IEntityTypeConfiguration<Car>
{
	public void Configure(EntityTypeBuilder<Car> builder)
	{
		builder.HasKey(car => car.Id)
			   .HasName("PK_Cars_Id");

		builder.Property(car => car.RegistrationPlate)
			   .HasColumnType("nvarchar(9)")
			   .IsRequired();

		builder.Property(car => car.ModelName)
			   .HasColumnType("nvarchar(40)")
			   .IsRequired();

		builder.HasIndex(car => car.RegistrationPlate)
			   .HasDatabaseName("IX_Cars_RegistrationPlate")
			   .IsUnique();

		builder.Property(car => car.Status)
			   .HasColumnType("nvarchar(12)")
			   .HasConversion(
				   status => status.ToString(),
				   status => (CarStatus)Enum.Parse(typeof(CarStatus), status))
			   .IsRequired();

		builder.HasOne(car => car.Driver)
			   .WithMany(driver => driver.Cars)
			   .HasForeignKey(car => car.DriverId)
			   .HasConstraintName("FK_Cars_DriverId")
			   .OnDelete(DeleteBehavior.NoAction)
			   .IsRequired();
	}
}
