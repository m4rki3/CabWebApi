using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CabWebApi.Domain.Core;
public class Driver : Person
{
	public int Salary { get; set; }
	public long DrivingLicense { get; set; }
	public IEnumerable<Car> Cars { get; set; }
}