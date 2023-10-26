using Application.Repository;
using Common.Dtos;
using Common.Entities;
using Common.Exceptions;
using Common.Mappings;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Lookups
{
    public class GetStatesByCountryHandler : IRequestHandler<GetStatesByCountryRequest, StateLookupResponseDto>
    {
        private readonly IQueryRepository<StateLookup> _repository;
        private readonly ILogger<GetAllStatesHandler> _logger;
        private readonly SharedMapping _mapping;

        public GetStatesByCountryHandler(IQueryRepository<StateLookup> repository, ILogger<GetAllStatesHandler> logger, SharedMapping mapping)
        {
            _repository = repository;
            _logger = logger;
            _mapping = mapping;
        }

        public async Task<StateLookupResponseDto> Handle(GetStatesByCountryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Getting states for {0}", request.CountryCode);
                StateLookupResponseDto responseDto = new StateLookupResponseDto();
                var states = _repository.Find(s => s.CountryCode == request.CountryCode);
                if (states != null)
                {
                    _mapping.Map(states, responseDto);
                }
                else
                {
                    string error = "No states found";
                    _logger.LogWarning(error);
                    responseDto.SetError(error);
                }
                return responseDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while getting states for country {0}", request.CountryCode);
                throw new DbException(ex.Message);
            }
        }
    }
}
