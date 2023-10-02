namespace Common.Dtos
{
    public class ApplicationPartnerResponseDto: BaseResponseDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public Guid UserGuid { get; set; }
        public string Email { get; set; }
        public string LogoUrl { get; set; }
        public string OrganizationName { get; set; }
        public string? Description { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public string PostCode { get; set; }
        public string CountryCode { get; set; }
        public bool IsLocked { get; set; }
    }
}
