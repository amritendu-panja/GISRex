namespace Common.Settings
{
    public class Constants
    {
        public const int SaltWorkFactor = 10;
        public const string JwtIdKey = "id";

        //Cache keys
        public const string AuthenticationCacheKey = "auth-key";
    }

    public enum TokenTypes
    {
        AccessToken = 1,
        RefreshToken = 2
    }
}
