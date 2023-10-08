using System.Linq.Expressions;

namespace Application.Repository
{
    public interface IRepository<T> 
        where T : class
    {
        Task<T> GetByIdAsync(int id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> expression);
        Task<T> AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        Task UpdateAsync(T entity);
        Task UpdateRangeAsync(IEnumerable<T> entities);
        Task Remove(T entity);
        Task RemoveRange(IEnumerable<T> entities);
        Task BeginTranscationAsync();
        Task CommitTransactionAsync();
        Task RollBackAsync();
    }
}
