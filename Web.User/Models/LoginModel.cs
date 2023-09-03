using System.ComponentModel.DataAnnotations;

namespace Web.User.Models
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "Enter Username")]
        public required string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Enter Password")]
        public required string Password { get; set; }        
    }
}
