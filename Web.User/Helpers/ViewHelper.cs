using Common.Dtos;
using Common.Settings;
using System.Security.Claims;

namespace Web.User.Helpers
{
    public class ViewHelper
    {
        private readonly CacheHelper _cacheHelper;
        private readonly CacheKeyGenrator _cachekeyGen;

        public ViewHelper(CacheHelper cacheHelper, CacheKeyGenrator cachekeyGen)
        {
            _cacheHelper = cacheHelper;
            _cachekeyGen = cachekeyGen;
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
                var userRole = GetUserRole(principal);
                if(userRole == RoleTypes.AppUser || userRole == RoleTypes.Administrator)
                {
                    var userDetails = _cacheHelper.Get<GetApplicationUserResponseDto>(userKey);
                    if (userDetails != null)
                    {
                        if (!string.IsNullOrEmpty(userDetails.FirstName) && !string.IsNullOrEmpty(userDetails.LastName))
                        {
                            return $"{userDetails.FirstName[0]}{userDetails.LastName[0]}";
                        }
                        if (!string.IsNullOrEmpty(userDetails.FirstName) && string.IsNullOrEmpty(userDetails.LastName))
                        {
                            return userDetails.FirstName;
                        }
                    }
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

        public Tuple<string, LoginResponseDto> GetLoginDetails(ClaimsPrincipal principal)
        {
            var key = _cachekeyGen.CreateCacheKey(principal, Constants.AuthenticationCacheKey);
            var loginData = _cacheHelper.Get<LoginResponseDto>(key);
            return new Tuple<string, LoginResponseDto>(key, loginData);
        }

        public string GetAccessToken(ClaimsPrincipal principal)
        {
            var key = _cachekeyGen.CreateCacheKey(principal, Constants.AuthenticationCacheKey);
            var loginData = _cacheHelper.Get<LoginResponseDto>(key);
            return loginData.AccessToken;
        }

        public string GetActiveLink(string currentLinkName, string activeLinkName)
        {
            return currentLinkName.Equals(activeLinkName, StringComparison.CurrentCultureIgnoreCase) ? "active" : string.Empty;
        }
    }
}
