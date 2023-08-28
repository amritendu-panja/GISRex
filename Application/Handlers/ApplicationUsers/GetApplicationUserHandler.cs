using Application.Repository;
using Common.Dtos;
using Common.Entities;
using Common.Mappings;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.ApplicationUsers
{
    public class GetApplicationUserHandler : IRequestHandler<GetApplicationUserRequest, ApplicationUserResponseDto>
    {
        private ILogger<GetApplicationUserHandler> logger;
        private readonly IApplicationUserRepository repository;
        private readonly SharedMapping sharedMapping;

        public GetApplicationUserHandler(IApplicationUserRepository repository, SharedMapping sharedMapping, ILogger<GetApplicationUserHandler> logger)
        {
            this.repository = repository;
            this.sharedMapping = sharedMapping;
            this.logger = logger;
        }

        public Task<ApplicationUserResponseDto> Handle(GetApplicationUserRequest request, CancellationToken cancellationToken)
        {
            logger.LogInformation($"Getting user data for {request.UserGuid}");
            var user = repository.Find(u => u.UserGuid == request.UserGuid).FirstOrDefault();
            if (user != null)
            {
                ApplicationUserResponseDto responseDto = new ApplicationUserResponseDto();
                sharedMapping.Map(user, responseDto);
                return Task.FromResult(responseDto);
            }
            else
            {
                logger.LogWarning($"User not found for {request.UserGuid}");
                return null;
            }
        }
    }
}
