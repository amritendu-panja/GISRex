using Application.Repository;
using Common.Dtos;
using Common.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.ApplicationPartners
{
	public class CheckDomainHandler: IRequestHandler<CheckDomainExistsRequest, BaseResponseDto>
	{
		private readonly ILogger<CheckDomainHandler> _logger;
		private readonly IApplicationPartnerOrganizationRepository _repository;

		public CheckDomainHandler(ILogger<CheckDomainHandler> logger, IApplicationPartnerOrganizationRepository repository)
		{
			_logger = logger;
			_repository = repository;
		}

		public async Task<BaseResponseDto> Handle(CheckDomainExistsRequest request, CancellationToken cancellationToken)
		{
			BaseResponseDto response = new BaseResponseDto();
			try
			{
				response.Success = _repository.IsDomainExists(request.Domain);				
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Error occured while checking domain {request.Domain}");
				throw new DbException(ex.Message);
			}
			return response;
		}
	}
}
