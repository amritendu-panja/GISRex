using MediatR;

namespace Common.Dtos
{
	public class GetMostRecentUsersRequest: IRequest<GetApplicationUserListResponseDto>
	{
		public int Count { get; set; } = 3;
	}
}
