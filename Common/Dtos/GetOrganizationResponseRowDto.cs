namespace Common.Dtos
{
    public class GetOrganizationResponseRowDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string? LogoUrl { get; set; }
        public string Email { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string DomainName { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsPasswordExpired { get; set; }
        public bool IsUserLocked { get; set; }        
        public string CountryCode { get; set; }
    }
}
