namespace Common.Dtos
{
    public class GetOrganizationUserResponseDto: GetApplicationUserResponseDto
    {
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set;}
        public string OrganizationLogo { get; set;}
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
    }
}
