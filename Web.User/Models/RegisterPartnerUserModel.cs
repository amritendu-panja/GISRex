using Common.Settings;
using System.ComponentModel.DataAnnotations;
using Web.User.Validators;

namespace Web.User.Models
{
    public class RegisterPartnerUserModel
    {        
        [Required(ErrorMessage = "Username is mandatory")]
        [MinLength(6, ErrorMessage = "Username must be at least 6 characters long")]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is mandatory")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$",
            ErrorMessage = "Password must contain 1 uppercase, 1 lowercase, 1 numeric and 1 special character")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm password is mandatory")]
        [Compare(nameof(Password), ErrorMessage = "Password and Confirm password must be same")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [CheckboxRequired(false, ErrorMessage = "Username already exists")]
        public bool IsUserExists { get; set; }

        [Required(ErrorMessage = "Email is mandatory")]
        [EmailAddress(ErrorMessage = "Enter valid email address")]
        public string Email { get; set; }
        public string? ImagePath { get; set; }
        public string? ImageData { get; set; }
        public string? ImageFilename { get; set; }

        [Required(ErrorMessage = "Firstname is mandatory")]
        [MinLength(3, ErrorMessage = "Firstname must be at least 3 characters long")]
        [Display(Name = "Firstname")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Lastname is mandatory")]
        [MinLength(3, ErrorMessage = "Lastname must be at least 3 characters long")]
        [Display(Name = "Lastname")]
        public string LastName { get; set; }

        [Display(Name = "Address line 1")]
        public string? AddressLine1 { get; set; }
        [Display(Name = "Address line 2")]
        public string? AddressLine2 { get; set; }
        public string? City { get; set; }
        [Display(Name = "State / Region")]
        public string? StateCode { get; set; }
        [Display(Name = "Postal code")]
        public string? PostCode { get; set; }
        public string? Mobile { get; set; }
        [Display(Name = "Alternate email address")]
        public string? AlternateEmail { get; set; }

        [Display(Name = "Country")]
        public string? CountryCode { get; set; }

        [Display(Name = "Alternate mobile")]
        public string? AlternateMobile { get; set; }

        public int OrganizationId { get; set; }
        public int RoleId { get; set; }

        public RegisterPartnerUserModel()
        {
            ImagePath = Constants.DefaultProfileImage;
        }
    }
}
