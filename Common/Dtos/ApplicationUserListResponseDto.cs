namespace Common.Dtos
{
	public class ApplicationUserListResponseDto: BaseResponseDto
	{
		public List<ApplicationUserListItemBaseDto> Users { get; set; }
	}
}
