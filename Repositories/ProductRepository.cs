using Clothings_Store.Data;
using Clothings_Store.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace Clothings_Store.Repositories;
public class ProductRepository : GenericRepository<Product, int>
{
    public ProductRepository(StoreContext context) : base(context)
    {
    }
    public override async Task CreateAsync(Product input)
    {
        var model = new Product();
        model.Name = input.Name;
        model.Code = input.Code;
        model.Sold = 0;
        model.Sale = input.Sale;
        model.CostPrice = input.CostPrice;
        model.UnitPrice = (input.Sale != null) ? (double)
            (input.CostPrice - (input.CostPrice * input.Sale / 100)) : 
            input.CostPrice;
        model.Picture = input.Picture;
        model.CategoryId = input.CategoryId;
        model.StockInDate = DateTime.Now;
        await base.CreateAsync(model);
    }
    public override async Task DeleteAsync(Product product)
    {
        await base.DeleteAsync(product);
    }
    public override async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _db.Set<Product>()
                        .Include(product => product.Category)
                        .AsNoTracking()
                        .ToListAsync();
    }

}
