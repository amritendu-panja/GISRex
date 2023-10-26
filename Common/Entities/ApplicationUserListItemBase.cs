namespace Common.Entities
{
	public class ApplicationUserListItemBase
    {
        public int UserId { get; private set; }
        public string UserName { get; private set; }
        public Guid UserGuid { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string ImagePath { get; private set; }
        public string CountryCode { get; private set; }
        public int RoleId { get; private set; }
        public string Role { get; private set; }
    }
}
