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

		public async Task<ApplicationOrganizationResponseDto> AddAsync(RegisterPartnerModel partnerModel, string accessToken, CancellationToken cancellationToken)
		{
			CreateApplicationOrganizationCommand command = new CreateApplicationOrganizationCommand();
			mapper.Map(partnerModel, command);
			return await _partnerApiClient.Add(command, appSettings.Security.ApiKey, accessToken, cancellationToken);
		}

		public async Task<ApplicationOrganizationListResponseDto> GetRecentPartners(string accessToken, CancellationToken cancellationToken)
		{
			return await _partnerApiClient.GetRecentPartners(appSettings.Security.ApiKey, accessToken, cancellationToken);
		}

        public async Task<ApplicationOrganizationResponseDto> GetByIdAsync(int id, string accessToken, CancellationToken cancellationToken)
        {
            return await _partnerApiClient.GetOrganizationbyId(id, appSettings.Security.ApiKey, accessToken, cancellationToken);
        }

		public async Task<BaseResponseDto> CheckDomainExistsAsync(string domain, string accessToken, CancellationToken cancellationToken)
		{
			return await _partnerApiClient.CheckDomainExists(domain, appSettings.Security.ApiKey, accessToken, cancellationToken);
		}

        public async Task<DataTableResponseBase<GetOrganizationResponseRowDto>> GetPartnerDataTableAsync(GetOrganizationsDataTableRequest request, string accessToken, CancellationToken cancellationToken)
        {
            return await _partnerApiClient.GetPartnerDataTable(request, appSettings.Security.ApiKey, accessToken, cancellationToken);
        }

		public async Task<OrganizationUserResponseDto> CreateOrgainizationUserAsync(CreateOrganizationUserCommand command, string accessToken, CancellationToken cancellationToken)
		{
            return await _partnerApiClient.CreateUser(command, appSettings.Security.ApiKey, accessToken, cancellationToken);
        }
    }
}
