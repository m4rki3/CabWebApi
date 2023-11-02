using CabWebApi.Domain.Core;
using System.ComponentModel.DataAnnotations;

namespace CabWebApi.Models;
public class OrderModel
{
	[Required(ErrorMessage = "This field is required")]
	[RegularExpression("[0-9]*", ErrorMessage = "User id contains incorrect symbols")]
	public int UserId { get; set; }

	[Required(ErrorMessage = "This field is required")]
	[RegularExpression("[0-9]*", ErrorMessage = "Car id contains incorrect symbols")]
	public int CarId { get; set; }

	[Required(ErrorMessage = "This object is required")]
	public Location Departure { get; set; } = null!;

	[Required(ErrorMessage = "This object is required")]
	public Location Destination { get; set; } = null!;

	[Required(ErrorMessage = "This field is required")]
	[RegularExpression("[0-9]*", ErrorMessage = "Price contains incorrect symbols")]
	public int Price { get; set; }
}
