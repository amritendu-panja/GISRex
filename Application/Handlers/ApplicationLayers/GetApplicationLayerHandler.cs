using Application.Repository;
using Common.Dtos;
using Common.Entities;
using Common.Exceptions;
using Common.Mappings;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.ApplicationLayers
{
    public class GetApplicationLayerHandler : IRequestHandler<GetApplicationLayerRequest, ApplicationLayerResponseDto>
    {
        private readonly IRepository<ApplicationLayer> repository;
        private readonly ILogger<GetApplicationLayerHandler> logger;
        private readonly SharedMapping sharedMapping;

        public GetApplicationLayerHandler(IRepository<ApplicationLayer> repository, ILogger<GetApplicationLayerHandler> logger, SharedMapping sharedMapping)
        {
            this.repository = repository;
            this.logger = logger;
            this.sharedMapping = sharedMapping;
        }

        public async Task<ApplicationLayerResponseDto> Handle(GetApplicationLayerRequest request, CancellationToken cancellationToken)
        {
            logger.LogInformation($"Fetching record for Layer : {request.Id}");
            ApplicationLayerResponseDto layerDto = new ApplicationLayerResponseDto();
            var layer = repository.Find(l => l.LayerId == request.Id).FirstOrDefault();
            if (layer != null)
            {
                layerDto.SetSuccess();
                sharedMapping.Map(layer, layerDto);                
            }
            else
            {
                string message = $"Layer not found {request.Id}";
                layerDto.SetError(message);
                logger.LogError(message);
            }
            return layerDto;
        }
    }
}
