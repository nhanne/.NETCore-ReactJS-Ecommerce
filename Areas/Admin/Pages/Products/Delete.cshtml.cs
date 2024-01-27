using Clothings_Store.Interface;
using Clothings_Store.Models.Database;
using Clothings_Store.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clothings_Store.Areas.Admin.Pages.Products;
[Authorize(Roles = "Shop Master")]
public class DeleteModel : PageModel
{
    private readonly IRepository<Product> _repository;
    public DeleteModel(IRepository<Product> repository) => _repository = repository;
    [TempData]
    public string? StatusMessage { get; set; }
    public IActionResult OnGet(int? Productid)
    {
        if (Productid == null) return NotFound("không tìm thấy sản phẩm");
        return Page();
    }
    public IActionResult OnPost(int Productid)
    {
        var model = _repository.Get(Productid);
        if (model == null) return NotFound("không tìm thấy sản phẩm");
        _repository.Delete(model);
        StatusMessage = $"Bạn vừa xóa sản phẩm: {model.Name}";
        return RedirectToPage("./Index");
    }
}
