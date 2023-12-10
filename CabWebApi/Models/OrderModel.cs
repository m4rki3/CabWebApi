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

	[Required(ErrorMessage = "This field is required")]
	public float DepartureLatitude { get; set; }

	[Required(ErrorMessage = "This field is required")]
	public float DepartureLongitude { get; set; }

	[Required(ErrorMessage = "This field is required")]
	public float DestinationLatitude { get; set; }

	[Required(ErrorMessage = "This field is required")]
	public float DestinationLongitude { get; set; }

	[Required(ErrorMessage = "This field is required")]
	[RegularExpression("[0-9]*", ErrorMessage = "Price contains incorrect symbols")]
	public int Price { get; set; }

	public OrderStatus Status { get; set; } = OrderStatus.Created;
}
