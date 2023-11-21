using MediatR;

namespace Common.Dtos
{
    public class GetMostRecentPartnersRequest : IRequest<GetOrganizationListResponseDto>
    {
        public int Count { get; set; } = 3;
    }
}
