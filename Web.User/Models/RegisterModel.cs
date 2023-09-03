using System.ComponentModel.DataAnnotations;

namespace Web.User.Models
{
    public class RegisterModel
    {
        [Required]
        [EmailAddress]
        [Display(Name ="Enter email address")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Enter Username")]
        public string Username { get; set; }
        [Required]
        [Display(Name = "Enter Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
