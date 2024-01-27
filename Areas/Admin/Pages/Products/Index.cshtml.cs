using Clothings_Store.Data;
using Clothings_Store.Interface;
using Clothings_Store.Models.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace Clothings_Store.Areas.Admin.Pages.Products;
[Authorize(Roles = "Shop Master")]
public class IndexModel : PageModel
{
    private readonly IRepository<Product> _repository;
    public IndexModel(IRepository<Product> repository) => _repository = repository;
    [TempData]
    public string StatusMessage { get; set; }
    public IEnumerable<Product> categories { get; set; }
    public async Task OnGet()
    {
        categories = _repository.GetAll();
    }
    public void onPost() => RedirectToPage();
}
