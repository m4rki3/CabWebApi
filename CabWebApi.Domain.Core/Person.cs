using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CabWebApi.Domain.Core;
public abstract class Person
{
	public int Id { get; set; }
	public string Name { get; set; }
	public string PhoneNumber { get; set; }
	public string Email { get; set; }
	public string Password { get; set; }
	public DateTime BirthDate { get; set; }
}