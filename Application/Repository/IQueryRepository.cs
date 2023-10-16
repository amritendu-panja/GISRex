using System.Linq.Expressions;

namespace Application.Repository
{
    public interface IQueryRepository<T>
        where T : class
    {
        Task<T> GetByIdAsync(int id);
        IEnumerable<T> GetAll();
        IQueryable<T> Find(Expression<Func<T, bool>> expression);
    }
}
