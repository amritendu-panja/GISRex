using Common.Dtos;
using Common.Settings;
using Microsoft.Extensions.Options;
using Web.User.Clients;
using Web.User.Models;

namespace Web.User.Services
{
    public class AuthService
    {
        private readonly IAuthenticationClient _client;
        private readonly ILogger<AuthService> _logger;
        private readonly AppSettings _appSettings;

        public AuthService(IAuthenticationClient client, ILogger<AuthService> logger, IOptions<AppSettings> appSettings)
        {
            _client = client;
            _logger = logger;
            _appSettings = appSettings.Value;
        }

        public async Task<LoginResponseDto> LoginAsync(string username, string password, CancellationToken cancellationToken)
        {
            LoginRequest request = new LoginRequest
            {
                Username = username,
                Password = password
            };

            return await _client.Login(request, _appSettings.Security.ApiKey, cancellationToken);
        }

        public async Task<GetApplicationUserResponseDto> RegisterAsync(string email, string userName, string firstName, string lastName, string password, int roleId, CancellationToken cancellationToken)
        {
            CreateApplicationUserCommand command = new CreateApplicationUserCommand()
            {
                Email = email,
                UserName = userName,
                Firstname = firstName,
                Lastname = lastName,
                PasswordSalt = password,
                Role = roleId
            };

            return await _client.Register(command, _appSettings.Security.ApiKey, cancellationToken);
        }

        public async Task<LogoutResponseDto> LogoutAsync(string userId, string accessToken, CancellationToken cancellationToken)
        {
            LogoutRequest logoutRequest = new LogoutRequest
            {
                UserId = Guid.Parse(userId)
            };

            return await _client.Logout(logoutRequest, _appSettings.Security.ApiKey, accessToken, cancellationToken);
        }

        public async Task<LoginResponseDto> RefreshAsync(string refreshToken, CancellationToken cancellationToken)
        {
            RefreshRequest refreshRequest = new RefreshRequest { RefreshToken = refreshToken };

            return await _client.Refresh(refreshRequest, _appSettings.Security.ApiKey, cancellationToken);
        }

        public async Task<GetApplicationUserResponseDto> ProfileAsync(string userId, string accessToken, CancellationToken cancellationToken)
        {
            var userGuid = Guid.Parse(userId);
            return await _client.Profile(userGuid, _appSettings.Security.ApiKey, accessToken, cancellationToken);
        }

        public async Task<GetApplicationUserResponseDto> UpdateProfileAsync(AppUserProfileModel model, string accessToken, CancellationToken cancellationToken)
        {
            UpdateApplicationUserProfileCommand command = new UpdateApplicationUserProfileCommand
            {
                 UserId = model.UserId,
                 UserName = model.UserName,
                 ImagePath = model.ImagePath,
                 FirstName = model.FirstName,
                 LastName = model.LastName,
                 AddressLine1 = model.AddressLine1,
                 AddressLine2 = model.AddressLine2,
                 City = model.City,
                 StateCode = model.StateCode,
                 PostCode = model.PostCode,
                 CountryCode = model.CountryCode,
                 Mobile = model.Mobile,
                 AlternateEmail = model.AlternateEmail,
                 AlternateMobile = model.AlternateMobile
            };
            return await _client.UpdateProfile(command, _appSettings.Security.ApiKey, accessToken, cancellationToken);
        }

        public async Task<LogoutResponseDto> ChangePasswordAsync(string userId, string oldPassword, string newPassword, string accessToken, CancellationToken cancellationToken)
        {
            ChangeUserPasswordCommand changeUserPasswordCommand = new ChangeUserPasswordCommand
            {
                UserId = Guid.Parse(userId), 
                OldPassword = oldPassword,
                NewPassword = newPassword
            };

            return await _client.ChangePassword(changeUserPasswordCommand, _appSettings.Security.ApiKey, accessToken, cancellationToken);
        }

        public async Task<GetApplicationUserResponseDto> CheckUserExists(string userName, CancellationToken cancellationToken)
        {
            return await _client.FindByUsername(userName, _appSettings.Security.ApiKey, cancellationToken);
        }

        public async Task<DataTableResponseBase<GetUserResponseRowDto>> GetUserDataTableAsync(GetUsersDataTableRequest request, string accessToken, CancellationToken cancellationToken)
        {
            return await _client.GetUserDataTable(request, _appSettings.Security.ApiKey, accessToken, cancellationToken);
        }

        public async Task<GetApplicationUserListResponseDto> GetRecentUsersAsync(string accessToken, CancellationToken cancellationToken)
        {
            return await _client.GetRecentUsers(_appSettings.Security.ApiKey, accessToken, cancellationToken);
        }
    }
}
