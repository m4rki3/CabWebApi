using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CabWebApi.Domain.Core;
public class Driver : User
{
    public int Salary { get; set; }
    public int DrivingLicense { get; set; }
    public IEnumerable<Car> Cars { get; set; }
}