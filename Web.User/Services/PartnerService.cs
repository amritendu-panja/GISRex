using Common.Dtos;
using Common.Settings;
using Microsoft.Extensions.Options;
using Web.User.Clients;
using Web.User.Helpers;
using Web.User.Models;

namespace Web.User.Services
{
	public class PartnerService
	{
		private readonly ILogger<PartnerService> _logger;
		private readonly IPartnerApiClient _partnerApiClient;
		private readonly AppSettings appSettings;
		private readonly Mapper mapper;

		public PartnerService(ILogger<PartnerService> logger, IPartnerApiClient partnerApiClient, IOptions<AppSettings> appSettings, Mapper mapper)
		{
			_logger = logger;
			_partnerApiClient = partnerApiClient;
			this.appSettings = appSettings.Value;
			this.mapper = mapper;
		}

		public async Task<ApplicationPartnerResponseDto> AddAsync(RegisterPartnerModel partnerModel, string accessToken, CancellationToken cancellationToken)
		{
			CreatePartnerCommand command = new CreatePartnerCommand();
			mapper.Map(partnerModel, command);
			return await _partnerApiClient.Add(command, appSettings.Security.ApiKey, accessToken, cancellationToken);
		}
	}
}
