using Common.Dtos;
using Common.Settings;
using Microsoft.Extensions.Options;
using Web.User.Clients;

namespace Web.User.Services
{
    public class LookupsService
    {
        private readonly ILookupsClient _client;
        private readonly ILogger<LookupsService> _logger;
        private readonly AppSettings _appSettings;

        public LookupsService(ILookupsClient client, ILogger<LookupsService> logger, IOptions<AppSettings> appSettings)
        {
            _client = client;
            _logger = logger;
            _appSettings = appSettings.Value;
        }

        public async Task<CountryLookupResponseDto> GetAllCountriesAsync(string accessToken, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting countries data");
            return await _client.GetAllCountries(_appSettings.Security.ApiKey, accessToken, cancellationToken);
        }

        public async Task<GetCallingCodeResponseDto> GetCallingCodeAsync(string countryCode, string accessToken, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting calling code for {0}", countryCode.Trim());
            return await _client.GetCallingCode(countryCode, _appSettings.Security.ApiKey, accessToken, cancellationToken);
        }

        public async Task<StateLookupResponseDto> GetAllStatesAsync(string accessToken, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting states data");
            return await _client.GetAllStates(_appSettings.Security.ApiKey, accessToken, cancellationToken);
        }

        public async Task<StateLookupResponseDto> GetStatesByCountryAsync(string countryCode, string accessToken, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting states data by country");
            return await _client.GetStatesByCountry(countryCode, _appSettings.Security.ApiKey, accessToken, cancellationToken);
        }
    }
}
