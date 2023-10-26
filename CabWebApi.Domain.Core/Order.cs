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
    public int CarId { get; set; }
    public Car Car { get; set; }
    public int DepartureId { get; set; }
    public Location Departure { get; set; }
    public int DestinationId { get; set; }
    public Location Destination { get; set; }
    public OrderStatus Status { get; set; }
    public int Price { get; set; }
}
