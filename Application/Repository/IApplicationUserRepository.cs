using Common.Entities;
using System.Linq.Expressions;

namespace Application.Repository
{
    public interface IApplicationUserRepository:IRepository<ApplicationUser>
    {
        bool IsEmailExists(string email);
        bool IsUsernameExists(string username);
        IEnumerable<ApplicationUser> FindWithDetails(Expression<Func<ApplicationUser, bool>> expression);
    }
}
