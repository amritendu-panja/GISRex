using Common.Settings;
using System.ComponentModel.DataAnnotations;

namespace Web.User.Models
{
    public class ProfileModel
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? ImagePath { get; set; }
        [Display(Name = "First name")]
        [MinLength(3, ErrorMessage = "First name must be at least 3 characters")]
        public string? ImageData { get; set; }
        public string? ImageFilename { get; set; }
        public string? FirstName { get; set; }
        [Display(Name = "Last name")]
        [MinLength(3, ErrorMessage = "Last name must be at least 3 characters")]
        public string? LastName { get; set; }
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

        public ProfileModel()
        {
            ImagePath = Constants.DefaultProfileImage;
        }
    }
}
