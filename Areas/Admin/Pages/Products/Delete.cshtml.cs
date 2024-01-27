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
    private readonly IRepository<Product, int> _repository;
    public DeleteModel(IRepository<Product, int> repository) => _repository = repository;
    [TempData]
    public string? StatusMessage { get; set; }
    public async Task<IActionResult> OnGet(int? Productid)
    {
        if (Productid == null) 
            return NotFound("không tìm thấy sản phẩm");
        await Task.CompletedTask;
        return Page();
    }
    public async Task<IActionResult> OnPost(int Productid)
    {
        var model = await _repository.GetByIdAsync(Productid);
        if (model == null) 
            return NotFound("không tìm thấy sản phẩm");
        await _repository.DeleteAsync(model);
        StatusMessage = $"Bạn vừa xóa sản phẩm: {model.Name}";
        return RedirectToPage("./Index");
    }
}
