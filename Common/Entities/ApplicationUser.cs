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
        public bool IsPasswordExpired { get; private set; }
        public bool IsUserLocked { get; private set; }
        public int RoleId { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public DateTime ModifiedDate { get; private set; }

        public ICollection<ApplicationLayer> ApplicationLayers { get; set; } = new List<ApplicationLayer>();
        public ICollection<SecurityTokenLog> SecurityTokenLogs { get; set; } = new List<SecurityTokenLog>();
        public ApplicationUserDetails? UserDetails { get; set; }
        public UserRoleLookup? Role {  get; set; }

        protected ApplicationUser() { }

        public ApplicationUser(string userName, string passwordSalt, string encryptedPassword, string email, int roleId)
        {
            UserName = userName;
            UserGuid = Guid.NewGuid();
            PasswordSalt = passwordSalt;
            PasswordEncrypted = encryptedPassword;
            Email = email;
            IsEnabled = true;
            IsPasswordExpired = false;
            IsUserLocked = false;
            RoleId = roleId;
            CreatedDate = DateTime.UtcNow;
            ModifiedDate = DateTime.UtcNow;
        }

        public void SetPassword(string password)
        {
            PasswordEncrypted = password;
            IsPasswordExpired = false;
            IsUserLocked = false;
            CreatedDate = CreatedDate.ToUniversalTime();
            ModifiedDate = DateTime.UtcNow;
        }

        public void SetLock(bool lockStatus)
        {
            IsUserLocked = lockStatus;
            CreatedDate = CreatedDate.ToUniversalTime();
            ModifiedDate = DateTime.UtcNow;
        }

        public void SetPasswordExpiration(bool isExpired)
        {
            IsPasswordExpired = isExpired;
            CreatedDate = CreatedDate.ToUniversalTime();
            ModifiedDate = DateTime.UtcNow;
        }

        public void SetEnabled(bool isEnabled)
        {
            IsEnabled = isEnabled;
            CreatedDate = CreatedDate.ToUniversalTime();
            ModifiedDate = DateTime.UtcNow;
        }
    }
}
