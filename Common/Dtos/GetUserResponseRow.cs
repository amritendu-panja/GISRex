namespace Common.Dtos
{
    public class GetUserResponseRow
    {
        public int UserId { get; set; }
        public required string UserName { get; set; }
        public string? Fullname { get; set; }
        public Guid UserGuid { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsPasswordExpired { get; set; }
        public bool IsUserLocked { get; set; }
        public int RoleId { get; set; }
    }
}
