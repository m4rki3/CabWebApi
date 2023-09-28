using Microsoft.EntityFrameworkCore;
using CabWebApi.Domain.Core;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CabWebApi.Infrastructure.Data.Configurations;
public class CarConfiguration : IEntityTypeConfiguration<Car>
{
    public void Configure(EntityTypeBuilder<Car> builder)
    {
        builder.HasKey(car => car.Id)
               .HasName("PK_Cars_Id");
               //.IsClustered();
        builder.Property(car => car.RegistrationNumber)
               .HasColumnType("nchar(6)")
               .IsRequired();
        builder.Property(car => car.ModelName)
               .HasColumnType("nvarchar(40)")
               .IsRequired();
        builder.HasIndex(car => car.RegistrationNumber)
               .HasDatabaseName("IX_Cars_RegistrationNumber")
               .IsUnique();
        builder.Property(car => car.Status)
               .HasColumnType("nvarchar(12)")
               .IsRequired();
    }
}
