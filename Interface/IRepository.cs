using System.Linq.Expressions;

namespace Clothings_Store.Interface;
public interface IRepository<T>
{
    T Get(int id);
    void Create(T entity);
    void Update(T entity);
    void Delete (T entity);
    IEnumerable<T> GetAll();
    IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
    void SaveChanges();
}
