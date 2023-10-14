using Application.Repository;
using Common.Dtos;
using Common.Exceptions;
using Common.Mappings;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.ApplicationPartners
{
    public class GetApplicationOrganizationByIdHandler : IRequestHandler<GetApplicationOrganizationByIdRequest, ApplicationOrganizationResponseDto>
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

        public async Task<ApplicationOrganizationResponseDto> Handle(GetApplicationOrganizationByIdRequest request, CancellationToken cancellationToken)
        {
            ApplicationOrganizationResponseDto responseDto = new ApplicationOrganizationResponseDto();
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
