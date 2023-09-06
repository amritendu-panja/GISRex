using System.ComponentModel.DataAnnotations;

namespace Web.User.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Email is mandatory")]
        [EmailAddress]
        [Display(Name ="Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Username is mandatory")]
        [Display(Name = "Username")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is mandatory")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm password is mandatory")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
