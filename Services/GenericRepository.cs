using Clothings_Store.Data;
using Clothings_Store.Interface;
using System.Linq.Expressions;

namespace Clothings_Store.Services
{
    public abstract class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly StoreContext _context;
        public GenericRepository(StoreContext context)
        {
            _context = context;
        }
        public virtual T Add(T entity)
        {
            return _context.Add(entity).Entity;
        }
        public virtual T Get(Guid id)
        {
            return _context.Find<T>(id)!;
        }
        public virtual IEnumerable<T> All()
        {
            return _context.Set<T>().ToList();
        }

        public virtual T Update(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate).ToList();
        }

        public virtual void SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}
