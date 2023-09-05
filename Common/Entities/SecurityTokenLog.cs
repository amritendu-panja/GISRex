using Common.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Entities
{
    public class SecurityTokenLog
    {
        public int LogId { get; private set; }
        public string Token { get; private set; }
        public int TokenType { get; private set; }
        public int UserId { get; private set; }
        public bool IsEnabled { get; private set; }
        public DateTime CreateDate { get; private set; }
        public DateTime ExpirationDate { get; private set; }
        public ApplicationUser User { get; set; }

        public SecurityTokenLog(string token, TokenTypes tokenType, int userId, DateTime expiration)
        {
            Token = token;
            TokenType = (int) tokenType;
            UserId = userId;
            ExpirationDate = expiration;
            CreateDate = DateTime.UtcNow;
            IsEnabled = true;
        }

        public void Disable()
        {
            IsEnabled = false;
            CreateDate = CreateDate.ToUniversalTime();
            ExpirationDate = ExpirationDate.ToUniversalTime();
        }

        protected SecurityTokenLog() { }
    }
}
