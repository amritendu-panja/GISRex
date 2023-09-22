using System.ComponentModel.DataAnnotations;

namespace Web.User.Models
{
    public class ChangePasswordModel
    {
        [Required]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Current password is mandatory")]        
        [Display(Name = "Current Password")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Display(Name = "New Password")]
        [Required(ErrorMessage = "New password is mandatory")]
        [MinLength(8, ErrorMessage = "New password must be at least 8 characters long")]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$",
            ErrorMessage = "Password must contain 1 uppercase, 1 lowercase, 1 numeric and 1 special character")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Display(Name = "Confirm new Password")]
        [Required(ErrorMessage = "Confirm password is mandatory")]
        [Compare(nameof(NewPassword), ErrorMessage = "New password and Confirm password must be same")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
