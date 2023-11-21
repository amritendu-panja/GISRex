using Application.Repository;
using Common.Dtos;
using Common.Entities;
using Common.Exceptions;
using Common.Mappings;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.OrganizationUsers
{
    public class UpdateOrganizationUserHandler : IRequestHandler<UpdateOrganizationUserProfileCommand, GetOrganizationUserResponseDto>
    {
        private readonly IApplicationUserRepository _userRepository;
        private readonly IRepository<ApplicationUserDetails> _detailsRepository;
        private readonly SharedMapping _mapping;
        private readonly ILogger<UpdateOrganizationUserHandler> _logger;

        public UpdateOrganizationUserHandler(
            IApplicationUserRepository userRepository, 
            IRepository<ApplicationUserDetails> repository, 
            SharedMapping mapping, 
            ILogger<UpdateOrganizationUserHandler> logger
            )
        {
            _userRepository = userRepository;
            _detailsRepository = repository;
            _mapping = mapping;
            _logger = logger;
        }

        public async Task<GetOrganizationUserResponseDto> Handle(UpdateOrganizationUserProfileCommand request, CancellationToken cancellationToken)
        {
            GetOrganizationUserResponseDto responseDto = new GetOrganizationUserResponseDto();
            try
            {
                var userEntity = _userRepository.FindWithOrganizationDetails(u => u.UserId ==  request.UserId).First();
                var userDetails = userEntity.UserDetails;

                _mapping.Map(request, userEntity);
                _mapping.Map(request, userDetails);

                await _userRepository.UpdateAsync(userEntity);
                await _detailsRepository.UpdateAsync(userDetails);

                userEntity = _userRepository.FindWithOrganizationDetails(u => u.UserId == request.UserId).First();
                _mapping.Map(userEntity, responseDto);
            }
            catch ( Exception ex )
            {
                _logger.LogError(ex, "Error occured while saving profile for {0}", request.UserName);
                throw new DbException(ex.Message);
            }
            return responseDto;
        }
    }
}
