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

        builder.Property(order => order.Price)
               .HasColumnType("smallint")
               .IsRequired();

        builder.Property(order => order.Status)
               .HasColumnType("nvarchar(15)")
               .HasConversion(
                   status => status.ToString(),
                   status => (OrderStatus)Enum.Parse(typeof(OrderStatus), status))
               .IsRequired();

        builder.HasOne(order => order.User)
               .WithMany(user => user.Orders)
               .HasForeignKey(order => order.UserId)
               .HasConstraintName("FK_Orders_UserId")
               .OnDelete(DeleteBehavior.NoAction)
               .IsRequired();

        builder.HasOne(order => order.Car)
               .WithMany(car => car.Orders)
               .HasForeignKey(order => order.CarId)
               .HasConstraintName("FK_Orders_CarId")
               .OnDelete(DeleteBehavior.NoAction)
               .IsRequired();

        builder.HasOne(order => order.Departure)
               .WithOne(location => location.Order)
               .HasForeignKey<Order>(order => order.DepartureId)
               .HasConstraintName("FK_Orders_DepartureId")
               .OnDelete(DeleteBehavior.NoAction)
               .IsRequired();

        builder.HasOne(order => order.Destination)
               .WithOne(location => location.Order)
               .HasForeignKey<Order>(order => order.DestinationId)
               .HasConstraintName("FK_Orders_DestinationId")
               .OnDelete(DeleteBehavior.NoAction)
               .IsRequired();
    }
}