using Common.Settings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Web.User.Helpers
{
    public class AuthHelper
    {
        private readonly AppSettings appSettings;
        private readonly ILogger<AuthHelper> logger;

        public AuthHelper(IOptions<AppSettings> appSettings, ILogger<AuthHelper> logger)
        {
            this.appSettings = appSettings.Value;
            this.logger = logger;
        }

        public async Task SignInAsync(string accessToken, HttpContext context)
        {
            var principal = ValidateToken(accessToken);
            var authProperties = new AuthenticationProperties
            {
                //AllowRefresh = <bool>,
                // Refreshing the authentication session should be allowed.

                //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                // The time at which the authentication ticket expires. A 
                // value set here overrides the ExpireTimeSpan option of 
                // CookieAuthenticationOptions set with AddCookie.

                //IsPersistent = true,
                // Whether the authentication session is persisted across 
                // multiple requests. When used with cookies, controls
                // whether the cookie's lifetime is absolute (matching the
                // lifetime of the authentication ticket) or session-based.

                //IssuedUtc = <DateTimeOffset>,
                // The time at which the authentication ticket was issued.

                //RedirectUri = <string>
                // The full path or absolute URI to be used as an http 
                // redirect response value.
            };

            await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);
        }

        public ClaimsPrincipal ValidateToken(string jwtToken)
        {
            TokenValidationParameters validationParameters = GetTokenValidationParameters();
            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out SecurityToken validatedToken);
            return principal;
        }

        public TokenValidationParameters GetTokenValidationParameters()
        {
            return new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(appSettings.Security.Authentication.AccessTokenSecret)),
                ValidIssuer = appSettings.Security.Authentication.Issuer,
                ValidAudience = appSettings.Security.Authentication.Audience,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ClockSkew = TimeSpan.Zero
            };
        }
    }
}
