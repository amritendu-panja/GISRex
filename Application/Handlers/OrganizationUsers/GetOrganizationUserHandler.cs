using Application.Repository;
using Common.Dtos;
using Common.Exceptions;
using Common.Mappings;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.OrganizationUsers
{
    public class GetOrganizationUserHandler : IRequestHandler<GetOrganizationUserRequest, GetOrganizationUserResponseDto>
    {
        private readonly IApplicationUserRepository _userRepository;
        private readonly SharedMapping _mapping;
        private readonly ILogger<GetOrganizationUserHandler> _logger;

        public GetOrganizationUserHandler(IApplicationUserRepository userRepository, SharedMapping mapping, ILogger<GetOrganizationUserHandler> logger)
        {
            _userRepository = userRepository;
            _mapping = mapping;
            _logger = logger;
        }

        public async Task<GetOrganizationUserResponseDto> Handle(GetOrganizationUserRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting organization user with Id: {0}", request.UserGuid);
            var response = new GetOrganizationUserResponseDto();
            try
            {
                var orgUser = _userRepository.FindWithOrganizationDetails(u => u.UserGuid == request.UserGuid).First();
                if (orgUser != null)
                {
                    _mapping.Map(orgUser, response);
                }
                else
                {
                    response.SetError($"No user found with Id: {request.UserGuid}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while creating user with Id: {0}", request.UserGuid);
                throw new DbException(ex.Message);
            }
            return response;
        }
    }
}
