using CabWebApi.Domain.Core;
using System.ComponentModel.DataAnnotations;

namespace CabWebApi.Models;
public class CarModel
{
	[Required(ErrorMessage = "This field is required")]
	[StringLength(maximumLength: 82, ErrorMessage = "Model name is too long")]
	[RegularExpression("[A-Za-zА-Яа-я0-9 -]*")]
	public string ModelName { get; set; }

	[Required(ErrorMessage = "This field is required")]
	[StringLength(maximumLength: 6, MinimumLength = 6)]
	[RegularExpression("[А-Яа-я]{1}[0-9]{3}[А-Яа-я]{2}",
		ErrorMessage = "Series or registration number are incorrect")]
	public string SeriesRegistrationNumber { get; set; }

	[Required(ErrorMessage = "This field is required")]
	[RegularExpression("[0-9]{2,3}", ErrorMessage = "Region code must contain 2 or 3 digits")]
	public int RegionCode { get; set; }

	[Required(ErrorMessage = "This field is required")]
	[RegularExpression("[0-9]*", ErrorMessage = "Driver id contains incorrect symbols")]
	public int DriverId { get; set; }

	public CarStatus Status { get; set; } = CarStatus.NotAvaliable;
}