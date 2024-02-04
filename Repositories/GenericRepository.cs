using Clothings_Store.Data;
using Clothings_Store.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Clothings_Store.Repositories;
public abstract class GenericRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class
{
    protected readonly StoreContext _db;
    public GenericRepository(StoreContext db)
    {
        _db = db;
    }
    public virtual async Task<TEntity?> GetByIdAsync(TKey id)
    {
        return await _db.FindAsync<TEntity>(id);
    }
    public virtual async Task CreateAsync(TEntity entity)
    {
        await _db.Set<TEntity>().AddAsync(entity);
        await _db.SaveChangesAsync();
    }
    public virtual async Task UpdateAsync(TEntity entity)
    {
        _db.Entry(entity).State = EntityState.Modified;
        await _db.SaveChangesAsync();
    }
    public virtual async Task DeleteAsync(TEntity entity)
    {
        _db.Set<TEntity>().Remove(entity);
        await _db.SaveChangesAsync();
    }
    public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _db.Set<TEntity>()
                        .AsQueryable()
                        .Where(predicate)
                        .AsNoTracking()
                        .ToListAsync();
    }
    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _db.Set<TEntity>()
                        .Select(item => item)
                        .AsNoTracking()
                        .ToListAsync();
    }
}

