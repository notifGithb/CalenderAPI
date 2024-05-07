using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ActivityCalender.DataAccess.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ActivityCalenderContext _context;

        public GenericRepository(ActivityCalenderContext context)
        {
            _context = context;
        }

        public DbSet<T> Entity => _context.Set<T>();

        public async Task AddAsync(T entity)
        {
            await Entity.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entity)
        {
            await Entity.AddRangeAsync(entity);
        }

        public void Delete(T entity)
        {
            Entity.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var entityToDelete = await GetByIdAsync(id);
            Delete(entityToDelete);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            Entity.RemoveRange(entities);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await Entity.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllWhereAsync(Expression<Func<T, bool>> predicate)
        {
            return await Entity.Where(predicate).ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await Entity.FindAsync(id);
        }

        public async Task<T> GetWhereAsync(Expression<Func<T, bool>> predicate)
        {
            return await Entity.FirstOrDefaultAsync(predicate);
        }

        public void Update(T entity)
        {
            Entity.Update(entity);
        }

        public void UpdateRange(IEnumerable<T> entity)
        {
            Entity.UpdateRange(entity);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
        {
            return await Entity.AnyAsync(expression);
        }

        public async Task<bool> ContainsAsync(T entity)
        {
            return await Entity.ContainsAsync(entity);
        }
    }
}
