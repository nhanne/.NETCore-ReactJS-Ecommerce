using Clothings_Store.Data;
using Clothings_Store.Models.Database;

namespace Clothings_Store.Repositories;
public class OrderRepository : GenericRepository<Order>
{
    public OrderRepository(StoreContext context) : base(context)
    {
    }
}
