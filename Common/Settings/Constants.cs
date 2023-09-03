namespace Common.Settings
{
    public class Constants
    {
        public const int SaltWorkFactor = 10;
        public const string JwtIdKey = "id";
    }

    public enum TokenTypes
    {
        AccessToken = 1,
        RefreshToken = 2
    }
}
