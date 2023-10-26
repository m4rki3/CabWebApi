using CabWebApi.Domain.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CabWebApi.Infrastructure.Data.Configurations;
public class LocationConfiguration : IEntityTypeConfiguration<Location>
{
	public void Configure(EntityTypeBuilder<Location> builder)
	{
		builder.HasKey(location => location.Id)
			   .HasName("PK_Locations_Id");

		builder.Property(locaiton => locaiton.Latitude)
			   .HasColumnType("decimal(8,5)")
			   .IsRequired();

		builder.Property(location => location.Longitude)
			   .HasColumnType("decimal(8,5)")
			   .IsRequired();

		builder.Property(location => location.Order)
			   .Metadata
			   .AddAnnotation(typeof(NotMappedAttribute).ToString(), null);
	}
}
