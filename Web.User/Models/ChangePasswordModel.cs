using System.ComponentModel.DataAnnotations;

namespace Web.User.Models
{
    public class ChangePasswordModel
    {
        [Required]
        public string UserId { get; set; }
        [Display(Name = "Current Password")]
        [Required]
        public string OldPassword { get; set; }
        [Display(Name = "New Password")]
        [Required]
        public string NewPassword { get; set; }
        [Display(Name = "Confirm new Password")]
        [Required]
        public string ConfirmPassword { get; set; }
    }
}
