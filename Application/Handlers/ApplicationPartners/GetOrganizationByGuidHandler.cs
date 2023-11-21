using Application.Repository;
using Common.Dtos;
using Common.Exceptions;
using Common.Mappings;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.ApplicationPartners
{
    public class GetOrganizationByGuidHandler : IRequestHandler<GetOrganizationByGuidRequest, GetApplicationOrganizationResponseDto>
    {
        private readonly IApplicationUserRepository _userRepository;
        private readonly IApplicationPartnerOrganizationRepository _organizationRepository;
        private readonly SharedMapping _mapping;
        private readonly ILogger<GetOrganizationByGuidHandler> _logger;

        public GetOrganizationByGuidHandler(
            IApplicationUserRepository userRepository,
            IApplicationPartnerOrganizationRepository organizationRepository, 
            SharedMapping mapping, 
            ILogger<GetOrganizationByGuidHandler> logger)
        {
            _userRepository = userRepository;
            _organizationRepository = organizationRepository;
            _mapping = mapping;
            _logger = logger;
        }

        public async Task<GetApplicationOrganizationResponseDto> Handle(GetOrganizationByGuidRequest request, CancellationToken cancellationToken)
        {
            GetApplicationOrganizationResponseDto responseDto = new GetApplicationOrganizationResponseDto();
            try
            {
                _logger.LogInformation("Getting Organization details for OrgGuid: {0}", request.OrgGuid);
                var user = _userRepository.Find(u => u.UserGuid == request.OrgGuid).First();
                var organization = _organizationRepository.FindWithDetails(o => o.OrganizationId == user.OrganizationId).FirstOrDefault();
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
                _logger.LogError(ex, "Error while getting Organization details  for OrgGuid: {0}", request.OrgGuid);
                throw new DbException(ex.Message);
            }
            return responseDto;
        }
    }
}
