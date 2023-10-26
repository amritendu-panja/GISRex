using MediatR;

namespace Common.Dtos
{
	public class GetMostRecentUsersRequest: IRequest<ApplicationUserListResponseDto>
	{
		public int Count { get; set; } = 3;
	}
}
