using System.ComponentModel.DataAnnotations;
using CabWebApi.Content.Attributes;
using Microsoft.EntityFrameworkCore;

namespace CabWebApi.Models;
public class UserRegistrationModel : UserModel
{
	[Required(ErrorMessage = "This field is required")]
	[Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
	[DataType(DataType.Password)]
	[UIHint("Password")]
	public string PasswordConfirm { get; set; }
}