using MediatR;

namespace Common.Dtos
{
    public class GetApplicationOrganizationByIdRequest: IRequest<ApplicationOrganizationResponseDto>
    {
        public int OrganizationId { get; set; }
    }
}
