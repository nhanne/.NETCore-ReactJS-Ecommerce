using Clothings_Store.Data;
using Clothings_Store.Models.Database;

namespace Clothings_Store.Repositories;
public class ProductsRepository : GenericRepository<Product>
{
    public ProductsRepository(StoreContext context) : base(context)
    {
    }
}
