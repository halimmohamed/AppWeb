using System.ComponentModel.DataAnnotations;
using WebAppSafeJourney.Models.Enums;

namespace WebAppSafeJourney.ViewModels
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "Full Name is required")]
        [Display(Name = "Full Name")]
        public string ?FullName { get; set; }

        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string ?Email { get; set; }

        [Display(Name = "Phone Number (Optional)")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string ?Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [Display(Name = "Confirm Password")]
        public string ?ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Please select your role")]
        public UserRole Role { get; set; }
    }
}