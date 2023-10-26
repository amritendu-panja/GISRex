using Application.Helpers;
using Application.Repository;
using Common.Dtos;
using Common.Exceptions;
using Common.Mappings;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.ApplicationUsers
{
	public class GetMostRecentlyUsedApplicationUsersHandler : IRequestHandler<GetMostRecentUsersRequest, ApplicationUserListResponseDto>
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

		public async Task<ApplicationUserListResponseDto> Handle(GetMostRecentUsersRequest request, CancellationToken cancellationToken)
		{
			ApplicationUserListResponseDto responseDto = new ApplicationUserListResponseDto();
			try
			{
				var query = await _fileHelper.GetFileContent("UserMruList");
				var userList = await _repository.GetMostRecentUsedUsers(query, request.Count);
				if (userList != null)
				{
					_sharedMapping.Map(userList, responseDto);
				}
				else
				{
					responseDto.Users = new List<ApplicationUserListItemBaseDto>();
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
