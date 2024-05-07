using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ActivityCalender.DataAccess.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        DbSet<T> Entity { get; }
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entity);

        void Update(T entity);
        void UpdateRange(IEnumerable<T> entity);

        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
        Task DeleteByIdAsync(int id);

        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllWhereAsync(Expression<Func<T, bool>> predicate);

        Task<T> GetByIdAsync(int id);
        Task<T> GetWhereAsync(Expression<Func<T, bool>> predicate);

        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
        Task<bool> ContainsAsync(T entity);
    }
}
