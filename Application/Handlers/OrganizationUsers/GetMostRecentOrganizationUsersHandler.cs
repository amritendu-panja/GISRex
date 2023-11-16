using Application.Helpers;
using Application.Repository;
using Common.Dtos;
using Common.Exceptions;
using Common.Mappings;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.OrganizationUsers
{
    public class GetMostRecentOrganizationUsersHandler : IRequestHandler<GetMostRecentOrganizationUsersRequest, GetOrganizationUserListResponseDto>
    { 
        private readonly IApplicationUserRepository _userRepository;
        private readonly ILogger<GetMostRecentOrganizationUsersHandler> _logger;
        private readonly SharedMapping _sharedMapping;
        private readonly IFileHelper _fileHelper;

        public GetMostRecentOrganizationUsersHandler(IApplicationUserRepository userRepository, ILogger<GetMostRecentOrganizationUsersHandler> logger, SharedMapping sharedMapping, IFileHelper fileHelper)
        {
            _userRepository = userRepository;
            _logger = logger;
            _sharedMapping = sharedMapping;
            _fileHelper = fileHelper;
        }

        public async Task<GetOrganizationUserListResponseDto> Handle(GetMostRecentOrganizationUsersRequest request, CancellationToken cancellationToken)
        {
            GetOrganizationUserListResponseDto responseDto = new GetOrganizationUserListResponseDto();
            try
            {
                var query = await _fileHelper.GetFileContent("OrgUserMruList");
                var userList = await _userRepository.GetMostRecentOpenedOrganizationUsers(query, request.OrganizationId, request.Count);
                if (userList != null)
                {
                    _sharedMapping.Map(userList, responseDto);
                }
                else
                {
                    responseDto.Users = new List<BaseOrganizationUserListItemDto>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while getting MRU users");
                throw new DbException(ex.Message);
            }
            return responseDto;
        }
    }
}
