using Application.Repository;
using Common.Entities;
using Infrastructure.Data.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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

        public override IEnumerable<ApplicationUser> Find(Expression<Func<ApplicationUser, bool>> expression)
        {
            return _context.Set<ApplicationUser>()
                .Where(expression)
                .Include(u => u.UserDetails)
                .Include(u => u.Role);
        }
    }
}
