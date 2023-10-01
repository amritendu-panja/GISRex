using Common.Dtos;
using Common.Settings;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Web.User.Helpers
{
    public class ViewHelper
    {
        private readonly CacheHelper _cacheHelper;

        public ViewHelper(CacheHelper cacheHelper)
        {
            _cacheHelper = cacheHelper;
        }

        public bool IsLoggedIn(ClaimsPrincipal principal)
        {
            return principal.Identity?.IsAuthenticated ?? false;
        }

        public string GetUserId(ClaimsPrincipal principal)
        {
            return principal.FindFirst(Constants.JwtIdKey)?.Value ?? string.Empty;
        }

        public string GetUsername(ClaimsPrincipal principal)
        {
            return principal.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
        }

        public string GetFullName(ClaimsPrincipal principal, HttpContext context)
        {
            var userKey = context.Session.GetString(Constants.LoggedInUserCachekey);
            if (!string.IsNullOrEmpty(userKey)) 
            {                
                var userDetails = _cacheHelper.Get<ApplicationUserResponseDto>(userKey);
                if (userDetails != null && !string.IsNullOrEmpty(userDetails.FirstName) && !string.IsNullOrEmpty(userDetails.LastName))
                {
                    return $"{userDetails.FirstName[0]}{userDetails.LastName[0]}";
                }
            }
            return GetUsername(principal);
        }

        public bool ShouldShowLoginRegisterLinks(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLowerInvariant();
            var canShowLogin = true;
            if (!string.IsNullOrEmpty(path))
            {
                canShowLogin = !(path.Contains("login") || path.Contains("register"));
            }
            return canShowLogin;
        }

        public RoleTypes GetUserRole(ClaimsPrincipal principal)
        {
            RoleTypes roleType = RoleTypes.AppUser;
            var role = principal.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
            if(!string.IsNullOrEmpty(role))
            {
                switch (role)
                {
                    case RoleTypeNames.AppUser:
                        roleType = RoleTypes.AppUser;
                        break;
                    case RoleTypeNames.Partner:
                        roleType = RoleTypes.Partner;
                        break;
                    case RoleTypeNames.PartnerUser:
                        roleType = RoleTypes.PartnerUser;
                        break;
                    case RoleTypeNames.Administrator:
                        roleType = RoleTypes.Administrator;
                        break;
                }
            }
            return roleType;
        }
    }
}
