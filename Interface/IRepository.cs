using System.Linq.Expressions;

namespace Clothings_Store.Interface
{
    public interface IRepository<T>
    {
        T Add(T entity);
        T Update(T entity);
        T Get(Guid id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        void SaveChanges();
    }
}
