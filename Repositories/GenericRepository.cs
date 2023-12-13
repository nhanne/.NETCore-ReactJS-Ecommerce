using Clothings_Store.Data;
using Clothings_Store.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Clothings_Store.Repositories;
public abstract class GenericRepository<T> : IRepository<T> where T : class
{
    protected readonly StoreContext _db;
    public GenericRepository(StoreContext db)
    {
        _db = db;
    }
    public virtual T Get(int id) => _db.Find<T>(id);
    public virtual void Create(T entity) => _db.Set<T>().Add(entity);
    public virtual void Update(T entity) => _db.Entry(entity).State = EntityState.Modified;
    public virtual void Delete(T entity) => _db.Set<T>().Remove(entity);
    public IEnumerable<T> Find(Expression<Func<T, bool>> predicate) => _db.Set<T>().AsQueryable().Where(predicate).ToList();
    public IEnumerable<T> GetAll() => _db.Set<T>().Select(item => item).AsNoTracking().ToList();
    public void SaveChanges() => _db.SaveChanges();
}

