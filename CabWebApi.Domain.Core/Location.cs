using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CabWebApi.Domain.Core;
public class Location
{
	public int Id { get; set; }
	public float Latitude { get; set; }
	public float Longitude { get; set; }
	public Order DepartureOrder { get; set; }
	public Order DestinationOrder { get; set; }
}