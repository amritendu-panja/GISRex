using Application.Repository;
using Common.Dtos;
using Common.Entities;
using Common.Mappings;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.ApplicationLayers
{
    public class CreateApplicationLayerHandler : IRequestHandler<CreateAplicationLayerCommand, GetApplicationLayerResponseDto>
    {
        private readonly IRepository<ApplicationLayer> _layerRepository;
        private readonly IApplicationUserRepository _userRepository;
        private readonly ILogger<CreateApplicationLayerHandler> _logger;
        private readonly SharedMapping _sharedMapping;

        public CreateApplicationLayerHandler(IRepository<ApplicationLayer> layerRepository, IApplicationUserRepository userRepository, ILogger<CreateApplicationLayerHandler> logger, SharedMapping sharedMapping)
        {
            _layerRepository = layerRepository;
            _userRepository = userRepository;
            _sharedMapping = sharedMapping;
            _logger = logger;
        }

        public async Task<GetApplicationLayerResponseDto> Handle(CreateAplicationLayerCommand request, CancellationToken cancellationToken)
        {
            GetApplicationLayerResponseDto layerDto = new GetApplicationLayerResponseDto();
            try
            {
                var layer = new ApplicationLayer(Guid.NewGuid(), request.Name, string.Empty, request.OwnerId, DateTime.UtcNow, DateTime.UtcNow);
                await _layerRepository.AddAsync(layer);
                _sharedMapping.Map(layer, layerDto);
                layerDto.SetSuccess();
                _logger.LogInformation($"New layer created: {layer.LayerId}");
            }
            catch(Exception ex)
            {
                string message = "Could not create new layer at the time.";
                layerDto.SetError(message);
                _logger.LogError(ex, message);
            }
            return layerDto;
        }
    }
}
