using System.ComponentModel.DataAnnotations;

namespace Web.User.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Email is mandatory")]
        [EmailAddress]
        [Display(Name = "Email")]        
        public string Email { get; set; }

        [Required(ErrorMessage = "Username is mandatory")]
        [MinLength(6, ErrorMessage = "Username must be at least 6 characters long")]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is mandatory")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        [Display(Name = "Password")]        
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm password is mandatory")]
        [Compare(nameof(Password), ErrorMessage = "Password and Confirm password must be same")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Range(typeof(bool), "False", "False", ErrorMessage = "Username already taken")]
        public bool IsUserExists { get; set; }

        [Range(typeof(bool), "True", "True", ErrorMessage = "Please accept the terms and conditions to proceed")]
        public bool IsAcceptedTermsAndConditions { get; set; }
    }
}
