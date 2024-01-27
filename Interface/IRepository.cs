using System.Linq.Expressions;
namespace Clothings_Store.Interface;
public interface IRepository<TEntity, TKey>
{
    Task<TEntity?> GetByIdAsync(TKey id);
    Task CreateAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync (TEntity entity);
    Task<IEnumerable<TEntity>?> GetAllAsync();
    Task<IEnumerable<TEntity>?> FindAsync(Expression<Func<TEntity, bool>> predicate);
}
