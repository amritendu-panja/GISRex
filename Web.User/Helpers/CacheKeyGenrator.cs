using Common.Exceptions;
using System.Security.Claims;

namespace Web.User.Helpers
{
    public class CacheKeyGenrator
    {
        public string CreateCacheKey(ClaimsPrincipal prinicpal, params string[] keys)
        {
            var userNameClaim = prinicpal.FindFirst(ClaimTypes.Name);
            if (userNameClaim != null)
            {
                var userName = userNameClaim.Value;
                return $"{string.Join("_", keys)}_{userName}";
            }
            else return string.Empty;
        }

        public string CreateCacheKey(params string[] keys)
        {
            return string.Join("_", keys);
        }
    }
}
