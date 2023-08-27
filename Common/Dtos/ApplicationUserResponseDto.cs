namespace Common.Dtos
{
    public class ApplicationUserResponseDto: BaseResponseDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public Guid UserGuid { get; set; }
        public string Email { get; set; }        
        public bool IsLocked { get; set; }
    }
}
