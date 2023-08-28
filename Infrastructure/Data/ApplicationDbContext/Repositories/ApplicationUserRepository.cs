using Application.Repository;
using Common.Entities;
using Infrastructure.Data.Repository;

namespace Infrastructure.Data.ApplicationDbContext.Repositories
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        public ApplicationUserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public bool IsEmailExists(string email)
        {
            return Find(u => u.Email.ToLower() == email).Any();
        }

        public bool IsUsernameExists(string username)
        {
            return Find(u => u.UserName.ToLower() == username).Any();
        }
    }
}
