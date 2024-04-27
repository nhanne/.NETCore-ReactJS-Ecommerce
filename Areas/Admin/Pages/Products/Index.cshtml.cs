using Clothings_Store.Interface;
using Clothings_Store.Models.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clothings_Store.Areas.Admin.Pages.Products;
[Authorize(Roles = "Shop Master")]
public class IndexModel : PageModel
{
    private readonly IRepository<Product, int> _repository;
    public IndexModel(IRepository<Product, int> repository) => _repository = repository;
    [TempData]
    public string? StatusMessage { get; set; }
    public IEnumerable<Product> Products { get; set; } = new List<Product>();
    public async Task OnGet()
    {
        Products = await _repository.GetAllAsync();
    }
    public void OnPost() => RedirectToPage();
}
