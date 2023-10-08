using MediatR;

namespace Common.Dtos
{
    public class GetMostRecentPartnersRequest : IRequest<ApplicationPartnerListResponseDto>
    {
        public int Count { get; set; } = 3;
    }
}
