using System.ComponentModel.DataAnnotations;

namespace Web.User.Models
{
    public class PartnerProfileModel
    {
        public int UserId { get; set; }
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }
        public Guid UserGuid { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string LogoUrl { get; set; }
		public string? ImageData { get; set; }
		public string? ImageFilename { get; set; }
		[Required]
        public int OrganizationId { get; set; }
        [Required]
        [Display(Name = "Organization Name")]
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
        [Display(Name = "Potal Code")]
        public string? PostCode { get; set; }
        [Required]
        [Display(Name = "Country")]
        public string CountryCode { get; set; }
        public bool IsLocked { get; set; }
    }
}
