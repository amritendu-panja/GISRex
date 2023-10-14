using Application.Repository;
using Common.Dtos;
using Common.Exceptions;
using Common.Mappings;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.ApplicationPartners
{
    public class GetMostRecentlyUsedApplicationPartnersHandler : IRequestHandler<GetMostRecentPartnersRequest, ApplicationOrganizationListResponseDto>
    {
        private readonly IApplicationPartnerOrganizationRepository _repository;
        private readonly ILogger<GetMostRecentlyUsedApplicationPartnersHandler> _logger;
        private readonly SharedMapping _sharedMapping;

        public GetMostRecentlyUsedApplicationPartnersHandler(
            IApplicationPartnerOrganizationRepository repository, 
            ILogger<GetMostRecentlyUsedApplicationPartnersHandler> logger,
            SharedMapping mapping)
        {
            _repository = repository;
            _logger = logger;
            _sharedMapping = mapping;
        }

        public async Task<ApplicationOrganizationListResponseDto> Handle(GetMostRecentPartnersRequest request, CancellationToken cancellationToken)
        {
            ApplicationOrganizationListResponseDto responseDto = new ApplicationOrganizationListResponseDto();
            try
            {
                var partnerList = await _repository.GetMostRecentPartners(request.Count);
                if (partnerList != null)
                {
                    _sharedMapping.Map(partnerList, responseDto);
                }
                else
                {
                    responseDto.Organizations = new List<BaseApplicationOrganizationListItemDto>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while getting MRU partners");
                throw new DbException(ex.Message);
            }
            return responseDto;
        }
    }
}
