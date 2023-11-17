using System.ComponentModel.DataAnnotations;

namespace CabWebApi.Models;
public class DriverModel : UserModel
{
	[Required(ErrorMessage = "This field is required")]
	public int Salary { get; set; }

	[Required(ErrorMessage = "This field is required")]
	[StringLength(maximumLength: 10, MinimumLength = 10,
		ErrorMessage = "Driving license must contain 10 digits")]
	public uint DrivingLicense { get; set; }
}