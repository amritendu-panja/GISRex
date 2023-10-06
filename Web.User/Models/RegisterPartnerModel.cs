using Common.Settings;
using System.ComponentModel.DataAnnotations;
using Web.User.Validators;

namespace Web.User.Models
{
    public class RegisterPartnerModel
    {        
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is mandatory")]
        [EmailAddress(ErrorMessage = "Enter valid email address")]
        [Display(Name = "Email")]
        public string Email { get; set; }

		[Display(Name = "Phone number")]
		public string Phone { get; set; }

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
        public string? LogoPath { get; set; }
        
        public string? ImageData { get; set; }
        public string? ImageFilename { get; set; }

        [Required]
        [Display(Name = "Organization name")]
        [MinLength(3, ErrorMessage = "Organization name must be at least 3 characters")]
        public string OrganizationName { get; set; }

        public string? Description { get; set; }
        
        [Display(Name = "Address line 1")]
        public string? AddressLine1 { get; set; }
        [Display(Name = "Address line 2")]
        public string? AddressLine2 { get; set; }
        public string? City { get; set; }
        [Display(Name = "State / Region")]
        public string? StateCode { get; set; }

        [Display(Name = "Postal code")]
        public string? PostCode { get; set; }

        [Required(ErrorMessage = "Country is mandatory")]
        [Display(Name = "Country of Incorporation")]
        public string? CountryCode { get; set; }

		[CheckboxRequired(false, ErrorMessage = "Username already exists")]
		public bool IsUserExists { get; set; }

		public RegisterPartnerModel()
        {
            LogoPath = Constants.DefaultProfileImage;
        }
    }
}
