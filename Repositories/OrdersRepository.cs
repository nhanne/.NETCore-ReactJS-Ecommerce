using Clothings_Store.Data;
using Clothings_Store.Models.Database;

namespace Clothings_Store.Repositories;
public class OrdersRepository : GenericRepository<Order>
{
    public OrdersRepository(StoreContext context) : base(context)
    {
    }
}
