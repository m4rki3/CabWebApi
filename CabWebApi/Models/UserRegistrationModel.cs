using System.ComponentModel.DataAnnotations;
using CabWebApi.Content.Attributes;
using Microsoft.EntityFrameworkCore;

namespace CabWebApi.Models;
public class UserRegistrationModel
{
    [Required(ErrorMessage = "This field is required")]
    [StringLength(maximumLength: 30, MinimumLength = 1,
        ErrorMessage = "Name is too long")]

    [RegularExpression("[A-Za-z -]{1,30}",
        ErrorMessage = "Name contains incorrect symbols")]

    [Display(Name = "Your name")]
    public string Name { get; set; } = null!;


    [Required(ErrorMessage = "This field is required")]
    [Display(Name = "Your birth date")]
    [DataType(DataType.Date)]
    [RequiredAge(18, 120, ErrorMessage = "You have to be older than 18")]
    public DateTime BirthDate { get; set; }


    [Required(ErrorMessage = "This field is required")]
    [Phone(ErrorMessage = "Phone number is incorrect")]
    [Display(Name = "Your phone number")]
    [DataType(DataType.PhoneNumber)]
    public string PhoneNumber { get; set; } = null!;


    [Required(ErrorMessage = "This field is required")]
    [EmailAddress(ErrorMessage = "Email is incorrect")]
    [Display(Name = "Your email")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;


    [Required(ErrorMessage = "This field is required")]
    [StringLength(20, MinimumLength = 6,
        ErrorMessage = "Password must be from 6 to 20 symbols")]

    [RegularExpression(@"[A-Za-z0-9_]{6,20}",
        ErrorMessage = "Password must contain only A-Z, a-z, 0-9, _ symbols")]

    [DataType(DataType.Password)]
    [UIHint("Password")]
    public string Password { get; set; } = null!;


    [Required(ErrorMessage = "This field is required")]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    [DataType(DataType.Password)]
    [UIHint("Password")]
    public string PasswordConfirm { get; set; } = null!;
}
