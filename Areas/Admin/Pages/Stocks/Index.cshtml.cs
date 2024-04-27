using Clothings_Store.Interface;
using Clothings_Store.Models.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clothings_Store.Areas.Admin.Pages.Stocks;
[Authorize(Roles = "Shop Master")]
public class IndexModel : PageModel
{
    private readonly IRepository<Stock, int> _repository;
    public IndexModel(IRepository<Stock, int> repository) => _repository = repository;
    
    public IEnumerable<Stock> Stocks { get; set; } = new List<Stock>();
    public async Task OnGet()
    {
        Stocks = await _repository.GetAllAsync();
    }
}