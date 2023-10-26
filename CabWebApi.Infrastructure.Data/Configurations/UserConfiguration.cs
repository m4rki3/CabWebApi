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
        builder.HasKey(client => client.Id)
               .HasName("PK_Clients_Id");
               //.IsClustered();

        builder.Property(client => client.Name)
               .HasColumnType("nvarchar(30)")
               .IsRequired();

        builder.Property(client => client.PhoneNumber)
               .HasColumnType("nchar(12)")
               .IsRequired();

        builder.HasIndex(client => client.PhoneNumber)
               .HasDatabaseName("IX_Clients_PhoneNumber")
               .IsUnique();

        builder.Property(client => client.Password)
               .HasColumnType("nvarchar(20)")
               .IsRequired();

        builder.Property(client => client.BirthDate)
               .HasColumnType("date")
               .IsRequired();
    }
}
