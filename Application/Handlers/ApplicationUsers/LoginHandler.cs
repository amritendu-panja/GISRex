using Application.Helpers;
using Application.Repository;
using Common.Dtos;
using Common.Entities;
using Common.Exceptions;
using Common.Settings;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Application.Handlers.ApplicationUsers
{
    public class LoginHandler : IRequestHandler<LoginRequest, LoginResponseDto>
    {
        private readonly ILogger<LoginHandler> _logger;
        private readonly IApplicationUserRepository _repository;
        private readonly IRepository<SecurityTokenLog> _securityLogRepository;
        private readonly IHashHelper _hashHelper;
        private readonly ITokenHelper _tokenHelper;
        private readonly AppSettings _appSettings;

        public LoginHandler(ILogger<LoginHandler> logger, 
            IApplicationUserRepository repository, 
            IHashHelper hashHelper, 
            ITokenHelper tokenHelper, 
            IRepository<SecurityTokenLog> securityLogRepository,
            IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            _repository = repository;
            _hashHelper = hashHelper;
            _tokenHelper = tokenHelper;
            _securityLogRepository = securityLogRepository;
            _appSettings = appSettings.Value;
        }

        public async Task<LoginResponseDto> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            var errorMessage = "Invalid username / password";
            LoginResponseDto response = new LoginResponseDto();
            var user = _repository.FindWithRole(u => u.UserName == request.Username).FirstOrDefault();
            if (user == null)
            {
                response.SetError(errorMessage);
                _logger.LogError(errorMessage);
            }
            else
            {
                if (!_hashHelper.VerifyPassword(request.Password, user.PasswordEncrypted))
                {
                    response.SetError(errorMessage);
                    _logger.LogError(errorMessage);
                }
                else
                {
                    try
                    {
                        _logger.LogInformation("Generating tokens");
                        response.AccessToken = _tokenHelper.GenerateAccessToken(user);
                        response.RefreshToken = _tokenHelper.GenerateRefreshToken();
                        response.ExpiresAt = DateTime.Now.AddMinutes(_appSettings.Security.Authentication.AccessTokenTTLInMinutes);
                        DateTime accessTokenExpiration = DateTime.UtcNow.AddMinutes(_appSettings.Security.Authentication.AccessTokenTTLInMinutes);
                        await _securityLogRepository.AddAsync(new SecurityTokenLog(response.AccessToken, TokenTypes.AccessToken, user.UserId, accessTokenExpiration));
                        DateTime refreshTokenExpiration = DateTime.UtcNow.AddMinutes(_appSettings.Security.Authentication.RefreshTokenTTLInMinutes);
                        await _securityLogRepository.AddAsync(new SecurityTokenLog(response.RefreshToken, TokenTypes.RefreshToken, user.UserId, refreshTokenExpiration));
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to Login user: {0}", request.Username);
                        throw new DbException(ex.Message);
                    }
                }
            }
            return response;
        }
    }
}
