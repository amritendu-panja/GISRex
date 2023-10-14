using MediatR;

namespace Common.Dtos
{
    public class GetMostRecentPartnersRequest : IRequest<ApplicationOrganizationListResponseDto>
    {
        public int Count { get; set; } = 3;
    }
}
