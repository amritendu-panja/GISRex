using System.Text;

namespace Common.Entities
{
    public class ApplicationUser
    {
        public int UserId { get; private set; }
        public string UserName { get; private set; }
        public Guid UserGuid { get; private set; }
        public string PasswordSalt { get; private set; }
        public string PasswordEncrypted { get; private set; }
        public string Email { get; private set; }
        public bool IsEnabled { get; private set; }        
        public bool IsPasswordExpired { get; private set;}
        public bool IsUserLocked { get; private set;}
        public DateTime CreatedDate { get; private set; }
        public DateTime ModifiedDate { get; private set;}

        public ICollection<ApplicationLayer> ApplicationLayers { get; set; } = new List<ApplicationLayer>();

        public ApplicationUser(string userName, string passwordSalt, string email)
        {
            UserName = userName;
            UserGuid = Guid.NewGuid();
            PasswordSalt = passwordSalt;
            PasswordEncrypted = Convert.ToBase64String(Encoding.UTF8.GetBytes(passwordSalt));
            Email = email;
            IsEnabled = true;
            IsPasswordExpired = false;
            IsUserLocked = false;
            CreatedDate = DateTime.UtcNow;
            ModifiedDate = DateTime.UtcNow;
        }

        public void SetLock(bool lockStatus)
        {
            IsUserLocked = lockStatus;
        }

        public void SetPasswordExpiration(bool isExpired)
        {
            IsPasswordExpired = isExpired;
        }

        public void SetEnabled(bool isEnabled)
        {
            IsEnabled = isEnabled;
        }
    }
}
