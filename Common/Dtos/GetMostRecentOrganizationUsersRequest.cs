using MediatR;

namespace Common.Dtos
{
    public class GetMostRecentOrganizationUsersRequest: IRequest<GetOrganizationUserListResponseDto>
    {
        public int OrganizationId { get; set; }
        public int Count {  get; set; }
    }
}
