using Application.Helpers;
using Common.Entities;
using Common.Settings;
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

        public JwtTokenHelper(IOptions<AppSettings> appSettings)
        {
            this.appSettings = appSettings.Value;
        }

        public string GenerateToken(ApplicationUser user)
        {
            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Security.Authentication.TokenSecret));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new List<Claim>()
            {
                new Claim("id", user.UserGuid.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            JwtSecurityToken securityToken = new JwtSecurityToken(
                appSettings.Security.Authentication.Issuer,
                appSettings.Security.Authentication.Audience,
                claims,
                DateTime.UtcNow,
                DateTime.UtcNow.AddMinutes(appSettings.Security.Authentication.TokenTTLInMinutes),
                credentials);

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
    }
}
