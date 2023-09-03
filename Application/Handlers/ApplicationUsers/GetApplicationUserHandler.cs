using Application.Repository;
using Common.Dtos;
using Common.Exceptions;
using Common.Mappings;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.ApplicationUsers
{
    public class GetApplicationUserHandler : IRequestHandler<GetApplicationUserRequest, ApplicationUserResponseDto>
    {
        private readonly ILogger<GetApplicationUserHandler> logger;
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
            if (user == null)
            {
                throw new BusinessLogicException($"User not found for {request.UserGuid}");
            }

            ApplicationUserResponseDto responseDto = new ApplicationUserResponseDto();
            sharedMapping.Map(user, responseDto);
            return Task.FromResult(responseDto);
        }
    }
}
