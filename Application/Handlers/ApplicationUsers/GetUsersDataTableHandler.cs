using Application.Repository;
using Common.Dtos;
using Common.Settings;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.ApplicationUsers
{
    public class GetUsersDataTableHandler : IRequestHandler<GetUsersDataTableRequest, DataTableResponseBase<GetUserResponseRowDto>>
	{
		private readonly ILogger<GetUsersDataTableHandler> _logger;		
		private readonly IDataTableRepository<GetUserResponseRowDto> _tableRepository;
		private readonly IApplicationUserRepository _applicationUserRepository;

		public GetUsersDataTableHandler(
			ILogger<GetUsersDataTableHandler> logger,
			IDataTableRepository<GetUserResponseRowDto> userRepository,
			IApplicationUserRepository applicationUserRepository
			)
		{
			_logger = logger;
			_tableRepository = userRepository;
			_applicationUserRepository = applicationUserRepository;
		}

		public async Task<DataTableResponseBase<GetUserResponseRowDto>> Handle(GetUsersDataTableRequest request, CancellationToken cancellationToken)
		{
			DataTableResponseBase<GetUserResponseRowDto> response = new DataTableResponseBase<GetUserResponseRowDto>();
			response.Data = new List<GetUserResponseRowDto>();
			response.Draw = request.Draw;
			try
			{
				var user = _applicationUserRepository.Find(u => u.UserGuid == request.UserGuid).FirstOrDefault();
				string queryFile = "get_application_users_datatable";
				List<DapperParameter> parameters = new List<DapperParameter>();
                if (user != null)
				{
					List<int> roleIds = new List<int>();
					if (user.RoleId == (int)RoleTypes.Administrator)
					{
						roleIds.Add((int)RoleTypes.AppUser);
						roleIds.Add((int)RoleTypes.PartnerUser);
                    }
					else if (user.RoleId == (int) RoleTypes.Partner || user.RoleId == (int) RoleTypes.PartnerUser)
					{
                        roleIds.Add((int)RoleTypes.PartnerUser);
                    }
					parameters.Add(new DapperParameter("@RoleIds", roleIds.ToArray(), DapperDbTypes.Object));
                    parameters.Add(new DapperParameter("@OrganizationId", user.OrganizationId, DapperDbTypes.Int32));
                }

				return await _tableRepository.Get(request, queryFile, parameters);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error occured when getting Users DataTable.");
			}
			return response;
		}
	}
}
