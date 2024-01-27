using Clothings_Store.Data;
using Clothings_Store.Models.Database;

namespace Clothings_Store.Repositories;
public class ProductRepository : GenericRepository<Product, int>
{
    public ProductRepository(StoreContext context) : base(context)
    {
    }
    public override async Task CreateAsync(Product input)
    {
        var model = new Product();
        await base.CreateAsync(model);
    }
    public override async Task DeleteAsync(Product product)
    {
        await base.DeleteAsync(product);
    }
}
