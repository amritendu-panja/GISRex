using MediatR;

namespace Common.Dtos
{
    public class GetOrganizationUserRequest: IRequest<GetOrganizationUserResponseDto>
    {
        public Guid UserGuid { get; set; }
    }
}
