using Application.Repository;
using System.Linq.Expressions;

namespace Infrastructure.Data.Repository
{
    public class QueryRepository<TEntity> : IQueryRepository<TEntity> where TEntity : class
    {
        protected readonly ApplicationDbContext.ApplicationDbContext _context;

        public QueryRepository(ApplicationDbContext.ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> expression)
        {
            return _context.Set<TEntity>().Where(expression);
        }
        public IEnumerable<TEntity> GetAll()
        {
            return _context.Set<TEntity>().ToList();
        }
        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }
    }
}
