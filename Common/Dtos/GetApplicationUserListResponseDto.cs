namespace Common.Dtos
{
	public class GetApplicationUserListResponseDto: BaseResponseDto
	{
		public List<BaseApplicationUserListItemDto> Users { get; set; }
	}
}
