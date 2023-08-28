using Application.Helpers;

namespace Infrastructure.Helpers
{
    public class HashHelper : IHashHelper
    {
        public string GenerateSalt()
        {
            return BCrypt.Net.BCrypt.GenerateSalt(Common.Settings.Constants.SaltWorkFactor);
        }

        public string HashPassword(string password, string salt)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, salt);
        }

        public bool VerifyPassword(string password, string encryptedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, encryptedPassword);
        }
    }
}
