using MediatR;

namespace Common.Dtos
{
    public class GetOrganizationByGuidRequest: IRequest<GetApplicationOrganizationResponseDto>
    {
        public Guid OrgGuid { get; set; }
    }
}
