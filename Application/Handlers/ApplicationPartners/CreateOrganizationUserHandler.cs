using Application.Helpers;
using Application.Repository;
using Common.Dtos;
using Common.Entities;
using Common.Exceptions;
using Common.Mappings;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.ApplicationPartners
{
    public class CreateOrganizationUserHandler : IRequestHandler<CreateOrganizationUserCommand, OrganizationUserResponseDto>
    {
        private readonly ILogger<CreateOrganizationUserHandler> _logger;
        private readonly IApplicationUserRepository _userRepository;
        private readonly IRepository<ApplicationUserDetails> _detailsRepository;
        private readonly SharedMapping _mapping;
        private readonly IHashHelper _hashHelper;

        public CreateOrganizationUserHandler(
            ILogger<CreateOrganizationUserHandler> logger, 
            IApplicationUserRepository userRepository, 
            IRepository<ApplicationUserDetails> detailsRepository, 
            SharedMapping mapping, 
            IHashHelper hashHelper)
        {
            _logger = logger;
            _userRepository = userRepository;
            _detailsRepository = detailsRepository;
            _mapping = mapping;
            _hashHelper = hashHelper;
        }

        public async Task<OrganizationUserResponseDto> Handle(CreateOrganizationUserCommand request, CancellationToken cancellationToken)
        {
            if (_userRepository.IsEmailExists(request.Email))
            {
                throw new BusinessLogicException("Email already registered.");
            }

            if (_userRepository.IsUsernameExists(request.UserName))
            {
                throw new BusinessLogicException("Username already taken");
            }

            _logger.LogInformation("Started creating new user.");
            await _userRepository.BeginTranscationAsync();
            var response = new OrganizationUserResponseDto();
            try
            {
                string salt = _hashHelper.GenerateSalt();
                string encryptedPassword = _hashHelper.HashPassword(request.PasswordSalt, salt);
                ApplicationUser applicationUser = new ApplicationUser(request.UserName, salt, encryptedPassword, request.Email, request.RoleId, (int)Common.Settings.Groups.OrganizationUser, request.OrganizationId);
                var newUser = await _userRepository.AddAsync(applicationUser);
                ApplicationUserDetails userDetails = new ApplicationUserDetails(
                    newUser.UserId, 
                    request.ImagePath,
                    request.FirstName, 
                    request.LastName,
                    request.AddressLine1,
                    request.AddressLine2,
                    request.City,
                    request.StateCode,
                    request.PostCode,
                    request.Mobile,
                    request.AlternateEmail,
                    request.CountryCode,
                    request.AlternateMobile);
                userDetails = await _detailsRepository.AddAsync(userDetails);

                var createdUser = _userRepository.FindWithDetails(u => u.UserId == newUser.UserId).First();

                _mapping.Map(createdUser, response);
                _logger.LogInformation("New organization user created.");
                await _userRepository.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while creating user with username {0}", request.UserName);
                await _userRepository.RollBackAsync();
                throw new DbException(ex.Message);
            }
            return response;
        }
    }
}
