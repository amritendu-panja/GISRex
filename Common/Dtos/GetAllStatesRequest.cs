using MediatR;

namespace Common.Dtos
{
    public class GetAllStatesRequest : IRequest<StateLookupResponseDto>
    {
    }
}
