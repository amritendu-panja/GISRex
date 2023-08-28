using Common.Entities;

namespace Application.Repository
{
    public interface IApplicationUserRepository:IRepository<ApplicationUser>
    {
        bool IsEmailExists(string email);
        bool IsUsernameExists(string username);
    }
}
