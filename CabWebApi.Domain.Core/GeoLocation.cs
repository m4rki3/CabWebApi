using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CabWebApi.Domain.Core;
public class GeoLocation
{
    public int Id { get; set; }
    public int Latitude { get; set; }
    public int Longitude { get; set; }
    public int Altitude { get; set; }
}
