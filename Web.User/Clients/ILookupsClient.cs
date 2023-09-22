using Common.Dtos;
using Refit;

namespace Web.User.Clients
{
    public interface ILookupsClient
    {
        [Get("/api/countries/all")]
        Task<CountryLookupResponseDto> GetAllCountries([Header("XApiKey")] string apiKey, [Authorize("Bearer")] string accessToken, CancellationToken cancellationToken);

        [Get("/api/countries/{countryCode}/callingcode")]
        Task<GetCallingCodeResponseDto> GetCallingCode(string countryCode, [Header("XApiKey")] string apiKey, [Authorize("Bearer")] string accessToken, CancellationToken cancellationToken);

        [Get("/api/states/all")]
        Task<StateLookupResponseDto> GetAllStates([Header("XApiKey")] string apiKey, [Authorize("Bearer")] string accessToken, CancellationToken cancellationToken);

        [Get("/api/states/{countryCode}")]
        Task<StateLookupResponseDto> GetStatesByCountry(string countryCode, [Header("XApiKey")] string apiKey, [Authorize("Bearer")] string accessToken, CancellationToken cancellationToken);
    }
}
