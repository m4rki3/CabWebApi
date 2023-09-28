using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CabWebApi.Domain.Core;
public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public int DriverId { get; set; }
    public Driver Driver { get; set; }
    public int CarId { get; set; }
    public Car Car { get; set; }
    public int DepartureId { get; set; }
    public GeoLocation Departure { get; set; }
    public int DestinationId { get; set; }
    public GeoLocation Destination { get; set; }
    public OrderStatus Status { get; set; }
    public decimal Price { get; set; }
}
