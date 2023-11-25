namespace Common.Dtos
{
    public class GetUserResponseRowDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string? ImageUrl { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid UserGuid { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsPasswordExpired { get; set; }
        public bool IsUserLocked { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string CountryCode { get; set; }
    }
}
