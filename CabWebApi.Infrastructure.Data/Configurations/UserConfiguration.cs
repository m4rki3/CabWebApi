using CabWebApi.Domain.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CabWebApi.Infrastructure.Data.Configurations;
public class UserConfiguration : IEntityTypeConfiguration<User>
{
	public void Configure(EntityTypeBuilder<User> builder)
	{
		builder.Property(user => user.Name)
			   .HasColumnType("nvarchar(30)")
			   .IsRequired();

		builder.Property(user => user.PhoneNumber)
			   .HasColumnType("nchar(12)")
			   .IsRequired();

		builder.Property(user => user.Email)
			   .HasColumnType("nvarchar(63)")
			   .IsRequired();

		builder.HasIndex(user => user.PhoneNumber)
			   .HasDatabaseName("IX_Users_PhoneNumber")
			   .IsUnique();

		builder.Property(user => user.Password)
			   .HasColumnType("nvarchar(100)")
			   .IsRequired();

		builder.Property(user => user.BirthDate)
			   .HasColumnType("date")
			   .IsRequired();
	}
}
