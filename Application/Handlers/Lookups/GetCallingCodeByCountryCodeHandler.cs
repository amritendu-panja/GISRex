using Application.Repository;
using Common.Dtos;
using Common.Entities;
using Common.Exceptions;
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
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while getting calling code for country {0}", request.CountryCode);
                throw new DbException(ex.Message);
            }
        }
    }
}
