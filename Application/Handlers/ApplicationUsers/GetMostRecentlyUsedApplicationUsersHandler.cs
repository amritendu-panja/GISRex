using Application.Helpers;
using Application.Repository;
using Common.Dtos;
using Common.Exceptions;
using Common.Mappings;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.ApplicationUsers
{
    public class GetMostRecentlyUsedApplicationUsersHandler : IRequestHandler<GetMostRecentUsersRequest, GetApplicationUserListResponseDto>
	{
		private readonly IApplicationUserRepository _repository;
		private readonly ILogger<GetMostRecentlyUsedApplicationUsersHandler> _logger;
		private readonly SharedMapping _sharedMapping;
		private readonly IFileHelper _fileHelper;

		public GetMostRecentlyUsedApplicationUsersHandler(
			IApplicationUserRepository repository, 
			ILogger<GetMostRecentlyUsedApplicationUsersHandler> logger, 
			SharedMapping sharedMapping, 
			IFileHelper fileHelper)
		{
			_repository = repository;
			_logger = logger;
			_sharedMapping = sharedMapping;
			_fileHelper = fileHelper;
		}

		public async Task<GetApplicationUserListResponseDto> Handle(GetMostRecentUsersRequest request, CancellationToken cancellationToken)
		{
			GetApplicationUserListResponseDto responseDto = new GetApplicationUserListResponseDto();
			try
			{
				var query = await _fileHelper.GetFileContent("UserMruList");
				var userList = await _repository.GetMostRecentOpenedApplicationUsers(query, request.Count);
				if (userList != null)
				{
					_sharedMapping.Map(userList, responseDto);
				}
				else
				{
					responseDto.Users = new List<BaseApplicationUserListItemDto>();
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
