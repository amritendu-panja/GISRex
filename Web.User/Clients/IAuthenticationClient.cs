using Common.Dtos;
using Refit;

namespace Web.User.Clients
{
    public interface IAuthenticationClient
    {
        [Post("/api/auth/register")]
        Task<ApplicationUserResponseDto> Register([Body] CreateApplicationUserCommand command, [Header("XApiKey")] string apiKey, CancellationToken cancellationToken);

        [Post("/api/auth/login")]
        Task<LoginResponseDto> Login([Body] LoginRequest request, [Header("XApiKey")] string apiKey, CancellationToken cancellationToken);

        [Post("/api/auth/logout")]
        Task<LogoutResponseDto> Logout([Body] LogoutRequest request, [Header("XApiKey")] string apiKey, [Authorize("Bearer")] string accessToken,  CancellationToken cancellationToken);

        [Post("/api/auth/refresh")]
        Task<LoginResponseDto> Refresh([Body] RefreshRequest request, [Header("XApiKey")] string apiKey, CancellationToken cancellationToken);

        [Get("/api/users/{userId}")]
        Task<ApplicationUserResponseDto> Profile(Guid userId, [Header("XApiKey")] string apiKey, [Authorize("Bearer")] string accessToken, CancellationToken cancellationToken);

        [Post("/api/users/profile/update")]
        Task<ApplicationUserResponseDto> UpdateProfile(UpdateProfileCommand command, [Header("XApiKey")] string apiKey, [Authorize("Bearer")] string accessToken, CancellationToken cancellationToken);

        [Post("/api/auth/changepassword")]
        Task<LogoutResponseDto> ChangePassword([Body] ChangeUserPasswordCommand command, [Header("XApiKey")] string apiKey, [Authorize("Bearer")] string accessToken, CancellationToken cancellationToken);

        [Get("/api/users/findname/{userName}")]
        Task<ApplicationUserResponseDto> FindByUsername(string userName, [Header("XApiKey")] string apiKey, CancellationToken cancellationToken);
        [Post("/api/users/datatable/all")]
        Task<DataTableResponseBase<GetUserResponseRowDto>> GetUserDataTable(GetUsersDataTableRequest request, [Header("XApiKey")] string apiKey, [Authorize("Bearer")] string accessToken, CancellationToken cancellationToken);
        [Get("/api/users/recent")]
        Task<ApplicationUserListResponseDto> GetRecentUsers([Header("XApiKey")] string apiKey, [Authorize("Bearer")] string accessToken, CancellationToken cancellationToken);
    }
}
