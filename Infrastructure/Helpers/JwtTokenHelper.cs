using Application.Helpers;
using Common.Entities;
using Common.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Helpers
{
    public class JwtTokenHelper : ITokenHelper
    {
        private readonly AppSettings appSettings;
        private readonly ILogger<JwtTokenHelper> logger;

        public JwtTokenHelper(IOptions<AppSettings> appSettings, ILogger<JwtTokenHelper> logger)
        {
            this.appSettings = appSettings.Value;
            this.logger = logger;
        }

        public string GenerateAccessToken(ApplicationUser user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(Constants.JwtIdKey, user.UserGuid.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role.Role)
            };

            return GenerateToken(appSettings.Security.Authentication.AccessTokenSecret, appSettings.Security.Authentication.AccessTokenTTLInMinutes, claims);
        }

        public string GenerateRefreshToken()
        {
            return GenerateToken(appSettings.Security.Authentication.RefreshTokenSecret, appSettings.Security.Authentication.RefreshTokenTTLInMinutes);
        }

        public bool ValidateRefreshToken(string refreshToken)
        {
            var validationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Security.Authentication.RefreshTokenSecret)),
                ValidIssuer = appSettings.Security.Authentication.Issuer,
                ValidAudience = appSettings.Security.Authentication.Audience,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                RoleClaimType = ClaimTypes.Role,
                ClockSkew = TimeSpan.Zero
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                tokenHandler.ValidateToken(refreshToken, validationParameters, out SecurityToken securityToken);
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return false;
            }
        }

        private string GenerateToken(string tokenSecret, int tokenTTL, IEnumerable<Claim>? claims = null)
        {
            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSecret));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken securityToken = new JwtSecurityToken(
                appSettings.Security.Authentication.Issuer,
                appSettings.Security.Authentication.Audience,
                claims,
                DateTime.UtcNow,
                DateTime.UtcNow.AddMinutes(tokenTTL),
                credentials);

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
    }
}
