using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Common.Dtos
{
    public class GetCallingCodeByCountryRequest: IRequest<GetCallingCodeResponseDto>
    {
        [Required]
        public string CountryCode { get; set; }
    }
}
