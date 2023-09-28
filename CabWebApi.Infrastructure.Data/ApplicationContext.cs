using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using CabWebApi.Domain.Core;
using CabWebApi.Infrastructure.Data.Configurations;

namespace CabWebApi.Infrastructure.Data;
public class ApplicationContext : DbContext
{
    public DbSet<Car> Cars { get; set; }
    public DbSet<Driver> Drivers { get; set; }
    public DbSet<User> Clients { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<GeoLocation> GeoLocations { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CarConfiguration());
        modelBuilder.ApplyConfiguration(new DriverConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new OrderConfiguration());
    }
}
