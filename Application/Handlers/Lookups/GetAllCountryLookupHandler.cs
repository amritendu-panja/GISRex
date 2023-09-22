using Application.Repository;
using Common.Dtos;
using Common.Entities;
using Common.Mappings;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Lookups
{
    internal class GetAllCountryLookupHandler : IRequestHandler<GetAllCountriesRequest, CountryLookupResponseDto>
    {
        private ILogger<GetAllCountryLookupHandler> _logger;
        private IQueryRepository<CountryLookup> _repository;
        private readonly SharedMapping _sharedMapping;

        public GetAllCountryLookupHandler(ILogger<GetAllCountryLookupHandler> logger, IQueryRepository<CountryLookup> repository, SharedMapping sharedMapping)
        {
            _logger = logger;
            _repository = repository;
            _sharedMapping = sharedMapping;
        }

        public Task<CountryLookupResponseDto> Handle(GetAllCountriesRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting countries from DB");
            var countriesList = _repository.GetAll();
            var response = new CountryLookupResponseDto();
            if (countriesList == null)
            {
                response.SetError("No countries fetched");
            }
            else
            {
                _sharedMapping.Map(countriesList, response);
            }
            
            return Task.FromResult(response);
        }
    }
}
