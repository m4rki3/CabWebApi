using System.ComponentModel.DataAnnotations;

namespace CabWebApi.Models;
public class LoginModel
{
	[Required(ErrorMessage = "This field is required")]
	[RegularExpression(@"^\+7[0-9]{10}", ErrorMessage = "Phone number is incorrect")]
	[DataType(DataType.PhoneNumber)]
	[Display(Name = "Phone number")]
	public string PhoneNumber { get; set; }


	[Required(ErrorMessage = "This field is required")]
	[StringLength(20, MinimumLength = 6,
		ErrorMessage = "Password must be from 6 to 20 symbols")]

	[RegularExpression(@"[A-Za-z0-9_]{6,20}",
		ErrorMessage = "Password must contain only A-Z, a-z, 0-9, _ symbols")]

	[DataType(DataType.Password)]
	[UIHint("Password")]
	public string Password { get; set; }
}