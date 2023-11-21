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
    public class RefreshTokenHandler : IRequestHandler<RefreshRequest, LoginResponseDto>
    {
        private readonly ILogger<RefreshTokenHandler> _logger;
        private readonly IApplicationUserRepository _repository;
        private readonly IRepository<SecurityTokenLog> _securityLogRepository;
        private readonly IHashHelper _hashHelper;
        private readonly ITokenHelper _tokenHelper;
        private readonly AppSettings _appSettings;

        public RefreshTokenHandler(ILogger<RefreshTokenHandler> logger,
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

        public async Task<LoginResponseDto> Handle(RefreshRequest request, CancellationToken cancellationToken)
        {
            try
            {
                bool isRefreshTokenValid = _tokenHelper.ValidateRefreshToken(request.RefreshToken);
                var errorMessage = "Invalid refresh token";
                if (!isRefreshTokenValid)
                {
                    throw new InvalidModelException(errorMessage);
                }

                LoginResponseDto response = new LoginResponseDto();
                var securityTokenLog = _securityLogRepository.Find(u => u.Token == request.RefreshToken && u.IsEnabled == true).FirstOrDefault();
                if (securityTokenLog == null)
                {
                    throw new InvalidModelException(errorMessage);
                }
                else
                {
                    var user = _repository.FindWithRole(u => u.UserId == securityTokenLog.UserId && u.IsEnabled == true).FirstOrDefault();
                    if (user == null)
                    {
                        throw new InvalidModelException(errorMessage);
                    }

                    //Generate new tokens
                    response.AccessToken = _tokenHelper.GenerateAccessToken(user);
                    response.RefreshToken = _tokenHelper.GenerateRefreshToken();
                    response.ExpiresAt = DateTime.Now.AddMinutes(_appSettings.Security.Authentication.AccessTokenTTLInMinutes);
                    DateTime accessTokenExpiration = DateTime.UtcNow.AddMinutes(_appSettings.Security.Authentication.AccessTokenTTLInMinutes);
                    await _securityLogRepository.AddAsync(new SecurityTokenLog(response.AccessToken, TokenTypes.AccessToken, user.UserId, accessTokenExpiration));
                    DateTime refreshTokenExpiration = DateTime.UtcNow.AddMinutes(_appSettings.Security.Authentication.RefreshTokenTTLInMinutes);
                    await _securityLogRepository.AddAsync(new SecurityTokenLog(response.RefreshToken, TokenTypes.RefreshToken, user.UserId, refreshTokenExpiration));

                    //Disable the existing token so that it cannot be used again
                    securityTokenLog.Disable();
                    await _securityLogRepository.UpdateAsync(securityTokenLog);
                }
                return response;
            }
            catch (InvalidModelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while generating AccessToken");
                throw new DbException(ex.Message);
            }
        }
    }
}
