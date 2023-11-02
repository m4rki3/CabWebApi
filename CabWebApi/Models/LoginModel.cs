using System.ComponentModel.DataAnnotations;

namespace CabWebApi.Models;
public class LoginModel
{
    [Required(ErrorMessage = "This field is required")]
    [Phone(ErrorMessage = "Phone number is incorrect")]
    [DataType(DataType.PhoneNumber)]
    [Display(Name = "Phone number")]
    public string PhoneNumber { get; set; }


    [Required(ErrorMessage = "This field is required")]
    [DataType(DataType.Password)]
    [RegularExpression(@"[A-Za-z0-9_]*]",
        ErrorMessage = "Password must contain only A-Z, a-z, 0-9, _ symbols")]

    [UIHint("Password")]
    public string Password { get; set; }
}
