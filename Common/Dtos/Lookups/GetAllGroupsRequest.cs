using MediatR;

namespace Common.Dtos
{
	public class GetAllGroupsRequest: IRequest<GroupLookupResponseDto>
	{
	}
}
