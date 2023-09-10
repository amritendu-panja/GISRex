using System.ComponentModel.DataAnnotations;

namespace Web.User.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Username cannot be blank")]
        [Display(Name = "Username")]
        public required string Username { get; set; }

        [Required(ErrorMessage = "Password cannot be blank")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public required string Password { get; set; }        
    }
}
