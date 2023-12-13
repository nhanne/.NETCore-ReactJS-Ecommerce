using Clothings_Store.Data;
using Clothings_Store.Models.Database;

namespace Clothings_Store.Repositories;
public class CustomerRepository : GenericRepository<Customer>
{
    public CustomerRepository(StoreContext context) : base(context)
    {
    }
}
