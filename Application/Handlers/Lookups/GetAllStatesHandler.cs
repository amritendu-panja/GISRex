using Application.Repository;
using Common.Dtos;
using Common.Entities;
using Common.Mappings;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Lookups
{
    public class GetAllStatesHandler : IRequestHandler<GetAllStatesRequest, StateLookupResponseDto>
    {
        private readonly IQueryRepository<StateLookup> _repository;
        private readonly ILogger<GetAllStatesHandler> _logger;
        private readonly SharedMapping _mapping;

        public GetAllStatesHandler(IQueryRepository<StateLookup> repository, ILogger<GetAllStatesHandler> logger, SharedMapping mapping)
        {
            _repository = repository;
            _logger = logger;
            _mapping = mapping;
        }

        public async Task<StateLookupResponseDto> Handle(GetAllStatesRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting all states");
            StateLookupResponseDto responseDto = new StateLookupResponseDto();
            var states = _repository.GetAll();
            if(states != null)
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
    }
}
