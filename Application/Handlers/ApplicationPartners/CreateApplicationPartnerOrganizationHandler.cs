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
    public class CreateApplicationPartnerOrganizationHandler : IRequestHandler<CreateApplicationOrganizationCommand, ApplicationOrganizationResponseDto>
    {
        private readonly ILogger<CreateApplicationPartnerOrganizationHandler> _logger;
        private readonly IApplicationUserRepository _repository;
        private readonly IApplicationPartnerOrganizationRepository _organizationRepository;
        private readonly SharedMapping _mapping;
        private readonly IHashHelper _hashHelper;

        public CreateApplicationPartnerOrganizationHandler(
            ILogger<CreateApplicationPartnerOrganizationHandler> logger, 
            IApplicationUserRepository repository, 
            IApplicationPartnerOrganizationRepository organizationRepository, 
            SharedMapping mapping, 
            IHashHelper hashHelper)
        {
            _logger = logger;
            _repository = repository;
            _organizationRepository = organizationRepository;
            _mapping = mapping;
            _hashHelper = hashHelper;
        }

        public async Task<ApplicationOrganizationResponseDto> Handle(CreateApplicationOrganizationCommand request, CancellationToken cancellationToken)
        {
            if (_repository.IsEmailExists(request.Email))
            {
                throw new BusinessLogicException("Email already registered.");
            }

            if (_repository.IsUsernameExists(request.UserName))
            {
                throw new BusinessLogicException("Username already taken");
            }

            if (_organizationRepository.IsPartnerNameExists(request.OrganizationName))
            {
                throw new BusinessLogicException("Organization name already in use");
            }

            _logger.LogInformation("Started creating new user.");
            ApplicationOrganizationResponseDto responseDto = new ApplicationOrganizationResponseDto();
            try
            {
                await _repository.BeginTranscationAsync();
                string salt = _hashHelper.GenerateSalt();
                string encryptedPassword = _hashHelper.HashPassword(request.PasswordSalt, salt);

                ApplicationUser applicationUser = new ApplicationUser(request.UserName, salt, encryptedPassword, request.Email, request.RoleId);
                applicationUser = await _repository.AddAsync(applicationUser);

                ApplicationPartnerOrganization organization = new ApplicationPartnerOrganization(
                    applicationUser.UserId,
                    request.OrganizationName,
                    request.Description,
                    request.LogoUrl,
                    request.Phone,
                    request.AddressLine1,
                    request.AddressLine2,
                    request.City,
                    request.StateCode,
                    request.PostCode,
                    request.CountryCode
                    );
                organization = await _organizationRepository.AddAsync(organization);

                applicationUser.SetOrganizationId(organization.OrganizationId);
                await _repository.UpdateAsync(applicationUser);

                applicationUser.PartnerOrganization = organization;
                _mapping.Map(applicationUser, responseDto);
                await _repository.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while creating Partner with username: {0}", request.UserName);
                await _repository.RollBackAsync();
                throw new DbException(ex.Message);
            }
            return responseDto;
        }
    }
}
