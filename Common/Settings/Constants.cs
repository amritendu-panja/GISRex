namespace Common.Settings
{
    public class Constants
    {
        public const int SaltWorkFactor = 10;
        public const string JwtIdKey = "id";

        //Cache keys
        public const string AuthenticationCacheKey = "auth-key";
        public const string AuthExpireHeaderKey = "Token-Expired";
        public const string LoggedInUserCachekey = "user-key";

        //Profile Keys
        public const string DefaultProfileImage = "profile-user.svg";
        public const string CountryListCacheKey = "country-cache";
        public const string StateListCacheKey = "state-country-cache";
    }

    public enum TokenTypes
    {
        AccessToken = 1,
        RefreshToken = 2
    }

    public enum RoleTypes
    {
        AppUser = 1,
        Partner = 2,
        PartnerUser = 3,
        Administrator = 4
    }

    public class RoleTypeNames
    {
        public const string AppUser = "AppUser";
        public const string Partner = "Partner";
        public const string PartnerUser = "PartnerUser";
        public const string Administrator = "Administrator";
    }
}
