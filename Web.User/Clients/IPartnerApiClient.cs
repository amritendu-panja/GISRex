using Common.Dtos;
using Refit;

namespace Web.User.Clients
{
    public interface IPartnerApiClient
	{
		[Post("/api/partners/add")]
		Task<GetApplicationOrganizationResponseDto> Add([Body] CreateApplicationOrganizationCommand command, [Header("XApiKey")] string apiKey, [Authorize("Bearer")] string accessToken, CancellationToken cancellationToken);

		[Get("/api/partners/recent")]
        Task<GetOrganizationListResponseDto> GetRecentPartners([Header("XApiKey")] string apiKey, [Authorize("Bearer")] string accessToken, CancellationToken cancellationToken);

        [Get("/api/partners/{id}")]
        Task<GetApplicationOrganizationResponseDto> GetOrganizationbyId(int id, [Header("XApiKey")] string apiKey, [Authorize("Bearer")] string accessToken, CancellationToken cancellationToken);

        [Get("/api/partners/org/{orgGuid}")]
        Task<GetApplicationOrganizationResponseDto> GetOrganizationbyGuid(string orgGuid, [Header("XApiKey")] string apiKey, [Authorize("Bearer")] string accessToken, CancellationToken cancellationToken);

        [Get("/api/partners/checkdomain/{domain}")]
		Task<BaseResponseDto> CheckDomainExists(string domain, [Header("XApiKey")] string apiKey, [Authorize("Bearer")] string accessToken, CancellationToken cancellationToken);
		
        [Post("/api/partners/datatable/all")]
        Task<DataTableResponseBase<GetOrganizationResponseRowDto>> GetPartnerDataTable(GetOrganizationsDataTableRequest request, [Header("XApiKey")] string apiKey, [Authorize("Bearer")] string accessToken, CancellationToken cancellationToken);

		[Post("/api/partners/user/create")]
		Task<GetOrganizationUserResponseDto> CreateUser([Body] CreateOrganizationUserCommand command, [Header("XApiKey")] string apiKey, [Authorize("Bearer")] string accessToken, CancellationToken cancellationToken);

        [Get("/api/partners/user/{userGuid}")]
        Task<GetOrganizationUserResponseDto> GetUserProfile(string userGuid, [Header("XApiKey")] string apiKey, [Authorize("Bearer")] string accessToken, CancellationToken cancellationToken);

		[Post("/api/partners/user/update")]
        Task<GetOrganizationUserResponseDto> UpdateUserProfile(UpdateOrganizationUserProfileCommand command, [Header("XApiKey")] string apiKey, [Authorize("Bearer")] string accessToken, CancellationToken cancellationToken);

        [Get("/api/partners/{partnerId}/users/recent?count={count}")]
        Task<GetOrganizationUserListResponseDto> GetRecentUsers(int partnerId, int count, [Header("XApiKey")] string apiKey, [Authorize("Bearer")] string accessToken, CancellationToken cancellationToken);
    }
}
