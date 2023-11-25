using MediatR;

namespace Common.Dtos
{
    public class GetAllCountriesRequest: IRequest<CountryLookupResponseDto>
    {
    }
}
