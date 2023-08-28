namespace Common.Settings
{
    public class AppSettings
    {
        public Security Security { get; set; }
    }

    public class Security
    {
        public string KeyHeader { get; set; }
        public string ApiKey { get; set; }
        public Authentication Authentication { get; set; }
    }

    public class Authentication
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string TokenSecret { get; set; }
        public int TokenTTLInMinutes { get; set; }
    }
}
