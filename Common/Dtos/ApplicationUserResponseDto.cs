namespace Common.Dtos
{
    public class ApplicationUserResponseDto : BaseResponseDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public Guid UserGuid { get; set; }
        public string Email { get; set; }
        public string ImagePath { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public string PostCode { get; set; }
        public string Mobile { get; set; }
        public string AlternateEmail { get; set; }
        public string CountryCode { get; set; }
        public string AlternateMobile { get; set; }
        public bool IsLocked { get; set; }
    }
}
