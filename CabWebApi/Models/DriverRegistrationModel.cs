using System.ComponentModel.DataAnnotations;

namespace CabWebApi.Models;
public class DriverRegistrationModel : UserRegistrationModel
{
	[Required(ErrorMessage = "This field is required")]
	[RegularExpression("[0-9]{4}",
		ErrorMessage = "Driving license series must contain 4 digits")]

	[Display(Name = "Your driving license series")]
	public int LicenseSeries { get; set; }

	[Required(ErrorMessage = "This field is required")]
	[RegularExpression("[0-9]{6}",
		ErrorMessage = "Driving license number must contain 6 digits")]

	[Display(Name = "Your driving license number")]
	public int LicenseNumber { get; set; }
}