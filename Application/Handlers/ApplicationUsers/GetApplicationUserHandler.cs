using Application.Repository;
using Common.Dtos;
using Common.Entities;
using Common.Exceptions;
using Common.Mappings;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.ApplicationUsers
{
    public class GetApplicationUserHandler : IRequestHandler<GetApplicationUserRequest, ApplicationUserResponseDto>
    {
        private readonly ILogger<GetApplicationUserHandler> _logger;
        private readonly IApplicationUserRepository _repository;        
        private readonly SharedMapping sharedMapping;

        public GetApplicationUserHandler(
            IApplicationUserRepository repository, 
            SharedMapping sharedMapping,
            ILogger<GetApplicationUserHandler> logger)
        {
            this._repository = repository;
            this.sharedMapping = sharedMapping;
            this._logger = logger;
        }

        public Task<ApplicationUserResponseDto> Handle(GetApplicationUserRequest request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Getting user data for {request.UserGuid}");
                var user = _repository.FindWithDetails(u => u.UserGuid == request.UserGuid).FirstOrDefault();
                ApplicationUserResponseDto responseDto = new ApplicationUserResponseDto();
                if (user != null)
                {
                    sharedMapping.Map(user, responseDto);
                }
                else
                {
                    responseDto.SetError($"User not found by UserGuid {request.UserGuid}");
                }
                return Task.FromResult(responseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while getting UserGuid {0}", request.UserGuid);
                throw new DbException(ex.Message);
            }
        }
    }
}
