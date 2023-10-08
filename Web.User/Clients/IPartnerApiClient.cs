using Common.Dtos;
using Refit;

namespace Web.User.Clients
{
	public interface IPartnerApiClient
	{
		[Post("/api/partners/add")]
		Task<ApplicationPartnerResponseDto> Add([Body] CreatePartnerCommand command, [Header("XApiKey")] string apiKey, [Authorize("Bearer")] string accessToken, CancellationToken cancellationToken);

		[Get("/api/partners/recent")]
        Task<ApplicationPartnerListResponseDto> GetRecentpartners([Header("XApiKey")] string apiKey, [Authorize("Bearer")] string accessToken, CancellationToken cancellationToken);
    }
}
