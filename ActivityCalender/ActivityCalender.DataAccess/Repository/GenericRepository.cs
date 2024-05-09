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
            await _context.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<T> entity)
        {
            await Entity.AddRangeAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Remove(T entity)
        {
            Entity.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveRange(IEnumerable<T> entities)
        {
            Entity.RemoveRange(entities);
            await _context.SaveChangesAsync();
        }

        public async Task Update(T entity)
        {
            Entity.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRange(IEnumerable<T> entity)
        {
            Entity.UpdateRange(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await Entity.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllWhereAsync(Expression<Func<T, bool>> predicate)
        {
            return await Entity.Where(predicate).AsNoTracking().ToListAsync();
        }

        public async Task<T?> GetWhereAsync(Expression<Func<T, bool>> predicate)
        {
            return await Entity.AsNoTracking().FirstOrDefaultAsync(predicate);
        }

        //belirli bir koşula göre kontrol 
        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
        {
            return await Entity.AnyAsync(expression);
        }

        //öğe referansıyla kontrol
        public async Task<bool> ContainsAsync(T entity)
        {
            return await Entity.ContainsAsync(entity);
        }
    }
}
