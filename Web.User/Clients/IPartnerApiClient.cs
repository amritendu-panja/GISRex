using Common.Dtos;
using Refit;

namespace Web.User.Clients
{
	public interface IPartnerApiClient
	{
		[Post("/api/partners/add")]
		Task<ApplicationOrganizationResponseDto> Add([Body] CreateApplicationOrganizationCommand command, [Header("XApiKey")] string apiKey, [Authorize("Bearer")] string accessToken, CancellationToken cancellationToken);

		[Get("/api/partners/recent")]
        Task<ApplicationOrganizationListResponseDto> GetRecentPartners([Header("XApiKey")] string apiKey, [Authorize("Bearer")] string accessToken, CancellationToken cancellationToken);

        [Get("/api/partners/{id}")]
        Task<ApplicationOrganizationResponseDto> GetOrganizationbyId(int id, [Header("XApiKey")] string apiKey, [Authorize("Bearer")] string accessToken, CancellationToken cancellationToken);

		[Get("/api/partners/checkdomain/{domain}")]
		Task<BaseResponseDto> CheckDomainExists(string domain, [Header("XApiKey")] string apiKey, [Authorize("Bearer")] string accessToken, CancellationToken cancellationToken);

        [Post("/api/partners/datatable/all")]
        Task<DataTableResponseBase<GetOrganizationResponseRowDto>> GetPartnerDataTable(GetOrganizationsDataTableRequest request, [Header("XApiKey")] string apiKey, [Authorize("Bearer")] string accessToken, CancellationToken cancellationToken);
    }
}
