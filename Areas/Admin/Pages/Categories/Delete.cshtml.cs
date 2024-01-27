using Clothings_Store.Interface;
using Clothings_Store.Models.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clothings_Store.Areas.Admin.Pages.Categories;
[Authorize(Roles = "Shop Master")]
public class DeleteModel : PageModel
{
    private readonly IRepository<Category, int> _repository;
    public DeleteModel(IRepository<Category,int> repository)
    {
        _repository = repository;
    }
    [TempData]
    public string? StatusMessage { get; set; }
    public async Task<IActionResult>? OnGet(int? categoryid)
    {
        if (categoryid == null) 
            return NotFound("không tìm thấy danh mục");
        await Task.CompletedTask;
        return Page();
    }
    public async Task<IActionResult> OnPost(int categoryid)
    {
        var model = await _repository.GetByIdAsync(categoryid);
        if (model == null) 
            return NotFound("không tìm thấy danh mục");
        await _repository.DeleteAsync(model);
        StatusMessage = $"Bạn vừa xóa danh mục: {model.Name}";
        return RedirectToPage("./Index");
    }
}
