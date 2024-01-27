using Clothings_Store.Data;
using Clothings_Store.Models.Database;

namespace Clothings_Store.Repositories;
public class ProductRepository : GenericRepository<Product>
{
    public ProductRepository(StoreContext context) : base(context)
    {
    }
    public override void Create(Product Input)
    {
        var model = new Product();
        model.Id = base._db.Products.ToList().Last().Id + 1;
        model.Name = Input.Name;
        model.Code = Input.Code;
        base.Create(model);
        base.SaveChanges();
    }
    public override void Delete(Product entity)
    {
        base.Delete(entity);
        base.SaveChanges();
    }
}
