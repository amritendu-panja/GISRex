namespace Application.Helpers
{
    public interface IHashHelper
    {
        string HashPassword(string  password, string salt);
        string GenerateSalt();

        bool VerifyPassword(string password, string encryptedPassword);
    }
}
