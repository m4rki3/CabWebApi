using System.ComponentModel.DataAnnotations;

namespace CabWebApi.Models;
public class UserRegistrationModel : UserModel
{
	[Required(ErrorMessage = "This field is required")]
	[Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
	[DataType(DataType.Password)]
	// ReSharper disable once Mvc.TemplateNotResolved
	[UIHint("Password")]
	public string PasswordConfirm { get; set; }
}