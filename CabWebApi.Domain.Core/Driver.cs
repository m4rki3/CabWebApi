using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CabWebApi.Domain.Core;
public class Driver
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Salary { get; set; }
    public byte Experience { get; set; }
    public IEnumerable<Car> Cars { get; set; }
}
