namespace Common.Dtos
{
    public class BaseApplicationOrganizationListItemDto
    {
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string? LogoUrl { get; set; }
        public string CountryCode { get; set; }
    }
}
