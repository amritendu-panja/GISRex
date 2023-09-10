using System.ComponentModel.DataAnnotations;

namespace Web.User.Models
{
    public class ChangePasswordModel
    {
        [Required]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Current password is mandatory")]        
        [Display(Name = "Current Password")]
        public string OldPassword { get; set; }

        [Display(Name = "New Password")]
        [Required(ErrorMessage = "New password is mandatory")]
        [MinLength(8, ErrorMessage = "New password must be at least 8 characters long")]
        public string NewPassword { get; set; }

        [Display(Name = "Confirm new Password")]
        [Required(ErrorMessage = "Confirm password is mandatory")]
        [Compare(nameof(NewPassword), ErrorMessage = "New password and Confirm password must be same")]
        public string ConfirmPassword { get; set; }
    }
}
