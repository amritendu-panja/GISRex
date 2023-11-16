namespace Common.Entities
{
    public class OrganizationUserListItemBase: ApplicationUserListItemBase
    {
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string LogoUrl { get; set; }
    }
}
