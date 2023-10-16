using Application.Repository;
using Common.Dtos;
using Common.Entities;
using Common.Extensions;
using Common.Mappings;
using Common.Settings;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.ApplicationUsers
{
	public class GetUsersDataTableHandler : IRequestHandler<GetUsersDataTableRequest, DataTableResponseBase<GetUserResponseRowDto>>
	{
		private readonly ILogger<GetUsersDataTableHandler> _logger;
		private readonly IApplicationUserRepository _repository;
		private readonly SharedMapping _mapping;

		public GetUsersDataTableHandler(ILogger<GetUsersDataTableHandler> logger, IApplicationUserRepository repository, SharedMapping mapping)
		{
			_logger = logger;
			_repository = repository;
			_mapping = mapping;
		}

		public async Task<DataTableResponseBase<GetUserResponseRowDto>> Handle(GetUsersDataTableRequest request, CancellationToken cancellationToken)
		{
			DataTableResponseBase<GetUserResponseRowDto> response = new DataTableResponseBase<GetUserResponseRowDto>();
			response.Data = new List<GetUserResponseRowDto>();
			response.Draw = request.Draw;
			try
			{
				IQueryable<ApplicationUser> userList = _repository.FindWithDetails(g => g.RoleId == (int)RoleTypes.AppUser);
                response.RecordsTotal = userList.Count();
                if (request.SearchValue != null && !string.IsNullOrEmpty(request.SearchValue))
				{
					var searchPattern = $"%{request.SearchValue.ToLower()}%";
					userList = userList.Where(g => EF.Functions.Like(g.UserName.ToLower(), searchPattern) ||
					 EF.Functions.Like(g.Email.ToLower(), searchPattern) ||
					 (g.UserDetails != null ? EF.Functions.Like(g.UserDetails.FirstName.ToLower(), searchPattern) : false) ||
					 (g.UserDetails != null ? EF.Functions.Like(g.UserDetails.LastName.ToLower(), searchPattern) : false));
				}

				if (request.SortDirection != null && request.SortDirection.Equals("desc", StringComparison.CurrentCultureIgnoreCase))
				{
					userList = userList.OrderByColumnDescending(request.SortColumn ?? "UserName");
				}
				else
				{
					userList = userList.OrderByColumn(request.SortColumn ?? "UserName");
				}
                response.RecordsFiltered = userList.Count();
                int skipValue = request.Start ?? 0;				
				userList = userList.Skip(skipValue).Take(request.PageSize);				
				if (userList != null && userList.Count() > 0)
				{
					foreach (var user in userList.ToList())
					{
						GetUserResponseRowDto dto = new GetUserResponseRowDto();
						_mapping.Map(user, dto);
						response.Data.Add(dto);
					}
				}
				else
				{
					string message = "No users found";
					_logger.LogError(message);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "No users found");
			}
			return response;
		}
	}
}
