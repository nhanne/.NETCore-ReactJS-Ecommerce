using Clothings_Store.Data;
using Clothings_Store.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace Clothings_Store.Repositories;
public class CategoriesRepository : GenericRepository<Category>
{
    public CategoriesRepository(StoreContext context) : base(context)
    {
    }
    public override void Create(Category Input)
    {
        var model = new Category();
        model.Id = base._db.Categories.ToList().Last().Id + 1;
        model.Name = Input.Name;
        model.Code = Input.Code;
        base.Create(model);
        base.SaveChanges();
    }
    public override void Delete(Category entity)
    {
        base.Delete(entity);
        base.SaveChanges();
    }
}
