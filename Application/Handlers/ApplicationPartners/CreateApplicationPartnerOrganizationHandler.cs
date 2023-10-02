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
    public class CreateApplicationPartnerOrganizationHandler : IRequestHandler<CreatePartnerCommand, ApplicationPartnerResponseDto>
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

        public async Task<ApplicationPartnerResponseDto> Handle(CreatePartnerCommand request, CancellationToken cancellationToken)
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
            string salt = _hashHelper.GenerateSalt();
            string encryptedPassword = _hashHelper.HashPassword(request.PasswordSalt, salt);
            ApplicationUser applicationUser = new ApplicationUser(request.UserName, salt, encryptedPassword, request.Email, request.RoleId);
            applicationUser = await _repository.AddAsync(applicationUser);

            ApplicationPartnerOrganization organization = new ApplicationPartnerOrganization(
                applicationUser.UserId,
                request.OrganizationName,
                request.Description,
                request.LogoUrl,
                request.AddressLine1,
                request.AddressLine2,
                request.City,
                request.StateCode,
                request.PostCode,
                request.CountryCode
                );
            organization = await _organizationRepository.AddAsync(organization);

            applicationUser.PartnerOrganization = organization;

            ApplicationPartnerResponseDto responseDto = new ApplicationPartnerResponseDto();
            _mapping.Map(applicationUser, responseDto);
            return responseDto;
        }
    }
}
