namespace Common.Settings
{
    public class Constants
    {
        public const int SaltWorkFactor = 10;
        public const string JwtIdKey = "id";

        //Cache keys
        public const string AuthenticationCacheKey = "auth-key";
        public const string AuthExpireHeaderKey = "Token-Expired";

        //Profile
        public const string DefaultProfileImage = "profile-user.svg";
        public const string CountryListCacheKey = "country-cache";
        public const string StateListCacheKey = "state-country-cache";
    }

    public enum TokenTypes
    {
        AccessToken = 1,
        RefreshToken = 2
    }
}
