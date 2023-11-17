using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CabWebApi.Domain.Core;
public class User : Person
{
	public IEnumerable<Order> Orders { get; set; }
}