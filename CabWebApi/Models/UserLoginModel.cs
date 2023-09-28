using System.ComponentModel.DataAnnotations;

namespace CabWebApi.Models;
public class UserLoginModel
{
    [Required(ErrorMessage = "You did not enter the phone number")]
    [Phone(ErrorMessage = "Phone number is incorrect")]
    [DataType(DataType.PhoneNumber)]
    [Display(Name = "Phone number")]
    public string PhoneNumber { get; set; }


    [Required(ErrorMessage = "You did not enter the password")]
    [DataType(DataType.Password)]
    [RegularExpression(@"[A-Za-z0-9_-", ErrorMessage = "")]
    [UIHint("Password")]
    public string Password { get; set; }
}
