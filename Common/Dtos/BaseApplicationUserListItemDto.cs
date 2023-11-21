namespace Common.Dtos
{
    public class BaseApplicationUserListItemDto
	{
		public int UserId { get; set; }
		public string UserName { get; set; }
		public Guid UserGuid { get; set; }
		public string FirstName { get; set; }
		public string LastName { get;  set; }
		public string ImagePath { get; set; }
		public string CountryCode { get; set; }
		public int RoleId { get; set; }
		public string RoleName { get; set; }
	}
}
