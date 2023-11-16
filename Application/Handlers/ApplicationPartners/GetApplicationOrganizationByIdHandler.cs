using Application.Repository;
using Common.Dtos;
using Common.Exceptions;
using Common.Mappings;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.ApplicationPartners
{
	public class GetApplicationOrganizationByIdHandler : IRequestHandler<GetApplicationOrganizationByIdRequest, GetApplicationOrganizationResponseDto>
    {
        private readonly IApplicationPartnerOrganizationRepository _organizationRepository;
        private readonly SharedMapping _mapping;
        private readonly ILogger<GetApplicationOrganizationByIdHandler> _logger;

        public GetApplicationOrganizationByIdHandler(IApplicationPartnerOrganizationRepository organizationRepository, SharedMapping mapping, ILogger<GetApplicationOrganizationByIdHandler> logger)
        {
            _organizationRepository = organizationRepository;
            _mapping = mapping;
            _logger = logger;
        }

        public async Task<GetApplicationOrganizationResponseDto> Handle(GetApplicationOrganizationByIdRequest request, CancellationToken cancellationToken)
        {
            GetApplicationOrganizationResponseDto responseDto = new GetApplicationOrganizationResponseDto();
            try
            {
                _logger.LogInformation("Getting Organization details for OrgId: {0}", request.OrganizationId);
                var organization = _organizationRepository.FindWithDetails(o => o.OrganizationId == request.OrganizationId).FirstOrDefault();
                if (organization == null)
                {
                    responseDto.SetError("Organization not found");
                }
                else
                {
                    _mapping.Map(organization, responseDto);
                }
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error while getting Organization details  for OrgId: {0}", request.OrganizationId);
                throw new DbException(ex.Message);
            }
            return responseDto;
        }
    }
}
