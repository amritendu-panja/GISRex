using System.ComponentModel.DataAnnotations;

namespace Web.User.Models
{
    public class ProfileModel
    {
        [Display(Name = "Username")]
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
