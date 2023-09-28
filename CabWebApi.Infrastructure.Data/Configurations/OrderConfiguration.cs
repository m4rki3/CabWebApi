using CabWebApi.Domain.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CabWebApi.Infrastructure.Data.Configurations;
public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(order => order.Id)
               .HasName("PK_Orders_Id");
               //.IsClustered();
        builder.Property(order => order.Price)
               .HasColumnType("smallint(5, 0)")
               .IsRequired();
        builder.Property(order => order.Status)
               .HasColumnType("nvarchar(15)")
               .IsRequired();
    }
}
