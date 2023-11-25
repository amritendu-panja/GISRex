namespace Common.Dtos
{
    public class BaseOrganizationUserListItemDto: BaseApplicationUserListItemDto
    {
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationLogo { get; set;}
    }
}
