using System.ComponentModel.DataAnnotations;

namespace Common.Entities
{
    public class ApplicationPartnerOrganization
    {
        public int OrganizationId { get; private set; }
        /// <summary>
        /// Refers to UserId in ApplicationUser
        /// </summary>
        public int PartnerId { get; private set; }
        [Required]
        public string OrganizationName { get; private set;}
        public string? LogoUrl { get; private set; }
        public string? Description { get; private set;}
        public string? AddressLine1 { get; private set; }
        public string? AddressLine2 { get; private set; }
        public string? City { get; private set; }
        public int? StateCode { get; private set; }
        public string? PostCode { get; private set; }
        public string? CountryCode { get; private set; }
        public DateTime? CreateDate { get; private set; }
        public DateTime? ModifiedDate { get; private set; }

        public ApplicationUser? User { get; set; }

        public ApplicationPartnerOrganization(int partnerId, 
            string organizationName, 
            string? description,
            string? logoUrl,
            string? addressLine1, 
            string? addressLine2, 
            string? city, 
            int? stateCode, 
            string? postCode, 
            string? countryCode)
        {
            PartnerId = partnerId;
            OrganizationName = organizationName;
            Description = description;
            LogoUrl = logoUrl;
            AddressLine1 = addressLine1;
            AddressLine2 = addressLine2;
            City = city;
            StateCode = stateCode;
            PostCode = postCode;
            CountryCode = countryCode;
            CreateDate = DateTime.UtcNow; 
            ModifiedDate = DateTime.UtcNow;
        }

        protected ApplicationPartnerOrganization()
        {
        }
    }
}
