using MediatR;

namespace Common.Dtos
{
    public class GetApplicationOrganizationByIdRequest: IRequest<GetApplicationOrganizationResponseDto>
    {
        public int OrganizationId { get; set; }
    }
}
