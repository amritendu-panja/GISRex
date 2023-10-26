using Application.Repository;
using Common.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.ApplicationUsers
{
    public class GetUsersDataTableHandler : IRequestHandler<GetUsersDataTableRequest, DataTableResponseBase<GetUserResponseRowDto>>
	{
		private readonly ILogger<GetUsersDataTableHandler> _logger;		
		private readonly IDataTableRepository<GetUserResponseRowDto> _userRepository;

		public GetUsersDataTableHandler(
			ILogger<GetUsersDataTableHandler> logger,
			IDataTableRepository<GetUserResponseRowDto> userRepository
			)
		{
			_logger = logger;
			_userRepository = userRepository;
		}

		public async Task<DataTableResponseBase<GetUserResponseRowDto>> Handle(GetUsersDataTableRequest request, CancellationToken cancellationToken)
		{
			DataTableResponseBase<GetUserResponseRowDto> response = new DataTableResponseBase<GetUserResponseRowDto>();
			response.Data = new List<GetUserResponseRowDto>();
			response.Draw = request.Draw;
			try
			{
				return await  _userRepository.Get(request, "get_application_users_datatable");				
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error occured when getting Users DataTable.");
			}
			return response;
		}
	}
}
