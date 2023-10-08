namespace Common.Dtos
{
    public class BaseApplicationPartnerListItemDto
    {
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string? LogoUrl { get; set; }
        public string CountryCode { get; set; }
    }
}
