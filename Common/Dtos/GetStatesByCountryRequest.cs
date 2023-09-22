using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Common.Dtos
{
    public class GetStatesByCountryRequest: IRequest<StateLookupResponseDto>
    {
        [Required]
        public required string CountryCode { get; set; }
    }
}
