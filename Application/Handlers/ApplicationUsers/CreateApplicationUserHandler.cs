using Application.Helpers;
using Application.Repository;
using Common.Dtos;
using Common.Entities;
using Common.Exceptions;
using Common.Mappings;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.ApplicationUsers
{
    public class CreateApplicationUserHandler : IRequestHandler<CreateApplicationUserCommand, ApplicationUserResponseDto>
    {
        private readonly ILogger<CreateApplicationUserHandler> _logger;
        private readonly IApplicationUserRepository _repository;
        private readonly SharedMapping _mapping;
        private readonly IHashHelper _hashHelper;

        public CreateApplicationUserHandler(IApplicationUserRepository repository, SharedMapping mapping, IHashHelper helper, ILogger<CreateApplicationUserHandler> logger)
        {
            _repository = repository;
            _mapping = mapping;
            _hashHelper = helper;
            _logger = logger;
        }

        public async Task<ApplicationUserResponseDto> Handle(CreateApplicationUserCommand request, CancellationToken cancellationToken)
        {
            if(_repository.IsEmailExists(request.Email))
            {
                throw new BusinessLogicException("Email already registered.");
            }

            if (_repository.IsUsernameExists(request.UserName))
            {
                throw new BusinessLogicException("Username already taken");
            }

            _logger.LogInformation("Started creating new user.");
            string salt = _hashHelper.GenerateSalt();
            string encryptedPassword = _hashHelper.HashPassword(request.PasswordSalt, salt);
            ApplicationUser applicationUser = new ApplicationUser(request.UserName, salt, encryptedPassword, request.Email);
            var newUser = await _repository.AddAsync(applicationUser);
            var response = new ApplicationUserResponseDto();
            _mapping.Map(newUser, response);
            _logger.LogInformation("New user created.");
            return response;
        }

        
    }
}
