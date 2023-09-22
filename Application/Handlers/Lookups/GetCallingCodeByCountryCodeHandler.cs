using Application.Repository;
using Common.Dtos;
using Common.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Lookups
{
    public class GetCallingCodeByCountryCodeHandler : IRequestHandler<GetCallingCodeByCountryRequest, GetCallingCodeResponseDto>
    {
        private readonly IQueryRepository<CountryLookup> _respository;
        private readonly ILogger<GetCallingCodeByCountryCodeHandler> _logger;

        public GetCallingCodeByCountryCodeHandler(IQueryRepository<CountryLookup> respository, ILogger<GetCallingCodeByCountryCodeHandler> logger)
        {
            _respository = respository;
            _logger = logger;
        }

        public async Task<GetCallingCodeResponseDto> Handle(GetCallingCodeByCountryRequest request, CancellationToken cancellationToken)
        {
            var countryLookup = _respository.Find(c => c.ISO3Code == request.CountryCode).FirstOrDefault();
            GetCallingCodeResponseDto response = new GetCallingCodeResponseDto();
            if (countryLookup == null)
            {
                response.SetError($"Calling code not found for country {request.CountryCode} ");
            }
            else
            {
                response.CallingCode = countryLookup.CallingCode;
            }
            return response;
        }
    }
}
