using Clothings_Store.Interface;
using Clothings_Store.Models.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clothings_Store.Areas.Admin.Pages.Categories;
[Authorize(Roles = "Shop Master")]
public class IndexModel : PageModel
{
    private readonly IRepository<Category, int> _repository;
    [TempData]
    public string? StatusMessage { get; set; }
    public IEnumerable<Category>? categories { get; set; } = new List<Category>();

    public IndexModel(IRepository<Category, int> repository)
    {
        _repository = repository;
    }
    public async Task OnGet()
    {
        categories = await _repository.GetAllAsync();
    }
    public void onPost() => RedirectToPage();
}
