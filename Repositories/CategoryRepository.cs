using Clothings_Store.Data;
using Clothings_Store.Models.Database;

namespace Clothings_Store.Repositories;
public class CategoryRepository : GenericRepository<Category, int>
{
    public CategoryRepository(StoreContext context) : base(context)
    {
    }
    public override async Task CreateAsync(Category input)
    {
        var model = new Category();
        model.Id = base._db.Categories.ToList().Last().Id + 1;
        model.Name = input.Name;
        model.Code = input.Code;
        await base.CreateAsync(model);
    }
    public override async Task DeleteAsync(Category category)
    {
        await base.DeleteAsync(category);
    }
}
