using Clothings_Store.Data;
using Clothings_Store.Models.Database;
using Clothings_Store.Services;
using Microsoft.EntityFrameworkCore;

namespace Clothings_Store.Areas.Admin.Pages.Categories;
public class CategoriesRepository : GenericRepository<Category>
{
    private readonly StoreContext _context;
    public CategoriesRepository(StoreContext context) : base(context)
    {
        _context = context;
    }
    public override Category Add(Category Input)
    {
        var model = new Category();
        model.Id = _context.Categories.ToList().Last().Id + 1;
        model.Name = Input.Name;
        model.Code = Input.Code;
        model = base.Add(model);
        base.SaveChanges();
        return model;
    }
}
