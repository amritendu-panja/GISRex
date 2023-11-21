using Common.Entities;
using System.Linq.Expressions;

namespace Application.Repository
{
    public interface IApplicationUserRepository:IRepository<ApplicationUser>
    {
        bool IsEmailExists(string email);
        bool IsUsernameExists(string username);
        IQueryable<ApplicationUser> FindWithDetails(Expression<Func<ApplicationUser, bool>> expression);

        IQueryable<ApplicationUser> FindWithOrganizationDetails(Expression<Func<ApplicationUser, bool>> expression);
        IQueryable<ApplicationUser> FindWithRole(Expression<Func<ApplicationUser, bool>> expression);
        Task<List<ApplicationUserListItemBase>> GetMostRecentOpenedApplicationUsers(string query, int count);
        Task<List<OrganizationUserListItemBase>> GetMostRecentOpenedOrganizationUsers(string query, int organizationId, int count);
    }
}
