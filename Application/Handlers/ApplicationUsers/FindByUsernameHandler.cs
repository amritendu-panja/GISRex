using Application.Repository;
using Common.Dtos;
using Common.Exceptions;
using Common.Mappings;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.ApplicationUsers
{
    public class FindByUsernameHandler : IRequestHandler<FindByUsernameRequest, ApplicationUserResponseDto>
    {
        private readonly ILogger<FindByUsernameHandler> _logger;
        private readonly IApplicationUserRepository _repository;
        private readonly SharedMapping _sharedMapping;

        public FindByUsernameHandler(
            ILogger<FindByUsernameHandler> logger,
            IApplicationUserRepository repository,
            SharedMapping sharedMapping)
        {
            this._logger = logger;
            this._repository = repository;
            this._sharedMapping = sharedMapping;
        }

        public Task<ApplicationUserResponseDto> Handle(FindByUsernameRequest request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Getting user data for {request.Username}");
                var user = _repository.Find(u => u.UserName == request.Username).FirstOrDefault();
                ApplicationUserResponseDto responseDto = new ApplicationUserResponseDto();
                if (user != null)
                {
                    _sharedMapping.Map(user, responseDto);
                }
                else
                {
                    responseDto.SetError($"User not found by username {request.Username}");
                }
                return Task.FromResult(responseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while getting user {0}", request.Username);
                throw new DbException(ex.Message);
            }
        }
    }
}
