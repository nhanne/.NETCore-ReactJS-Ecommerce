using Clothings_Store.Data;
using Clothings_Store.Models.Database;

namespace Clothings_Store.Repositories;
public class OrderDetailsRepository : GenericRepository<OrderDetail>
{
    public OrderDetailsRepository(StoreContext context) : base(context)
    {
    }
}
