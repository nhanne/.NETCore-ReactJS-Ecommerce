using Clothings_Store.Data;
using Clothings_Store.Interface;
using Clothings_Store.Models.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clothings_Store.Areas.Admin.Pages.Categories;
[Authorize(Roles = "Shop Master")]
public class IndexModel : PageModel
{
    private readonly IRepository<Category> _categoriesRepository;
    [TempData]
    public string StatusMessage { get; set; }

    public IndexModel(IRepository<Category> categoriesRepository)
    {
        _categoriesRepository = categoriesRepository;
    }
    public IEnumerable<Category> categories { get; set; }
    public async Task OnGet()
    {
        categories = _categoriesRepository.GetAll();
    }
    public void onPost() => RedirectToPage();
}
