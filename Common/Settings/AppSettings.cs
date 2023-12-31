﻿namespace Common.Settings
{
    public class AppSettings
    {
        public Security Security { get; set; }
        public Cache Cache { get; set; }
        public Partners Partners { get; set; }
        public Users Users { get; set; }
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
        public string AccessTokenSecret { get; set; }
        public int AccessTokenTTLInMinutes { get; set; }
        public string RefreshTokenSecret { get; set; }
        public int RefreshTokenTTLInMinutes { get; set; }
    }

    public class Cache
    {
        public int SessionTimeoutInHours { get; set; }
        public int TimeoutInMinutes { get; set; }
    }

    public class Partners
    {
        public int MruListCount { get; set; }
    }

    public class Users
    {
        public int MruListCount { get; set; }
    }
}
