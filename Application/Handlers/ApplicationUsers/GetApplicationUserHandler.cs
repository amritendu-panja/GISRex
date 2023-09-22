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
        private readonly ILogger<GetApplicationUserHandler> logger;
        private readonly IApplicationUserRepository repository;
        private readonly IRepository<ApplicationUserDetails> detailsRepository;
        private readonly SharedMapping sharedMapping;

        public GetApplicationUserHandler(
            IApplicationUserRepository repository, 
            SharedMapping sharedMapping, 
            IRepository<ApplicationUserDetails> detailsRepository,
            ILogger<GetApplicationUserHandler> logger)
        {
            this.repository = repository;
            this.sharedMapping = sharedMapping;
            this.logger = logger;
            this.detailsRepository = detailsRepository;
        }

        public Task<ApplicationUserResponseDto> Handle(GetApplicationUserRequest request, CancellationToken cancellationToken)
        {
            logger.LogInformation($"Getting user data for {request.UserGuid}");
            var user = repository.Find(u => u.UserGuid == request.UserGuid).FirstOrDefault();
            if (user == null)
            {
                throw new BusinessLogicException($"User not found for {request.UserGuid}");
            }
            var details = detailsRepository.Find(u => u.UserId == user.UserId).FirstOrDefault();
            if(details != null)
            {
                user.UserDetails = details;
            }
            ApplicationUserResponseDto responseDto = new ApplicationUserResponseDto();
            sharedMapping.Map(user, responseDto);
            return Task.FromResult(responseDto);
        }
    }
}
