using Clothings_Store.Interface;
using Clothings_Store.Models.Database;
using Clothings_Store.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clothings_Store.Areas.Admin.Pages.Categories;
[Authorize(Roles = "Shop Master")]
public class DeleteModel : PageModel
{
    private readonly IRepository<Category> _repository;
    public DeleteModel(IRepository<Category> repository)
    {
        _repository = repository;
    }
    [TempData]
    public string StatusMessage { get; set; }
    public IActionResult OnGet(int categoryid)
    {
        if (categoryid == null) return NotFound("không tìm thấy danh mục");
        return Page();
    }
    public IActionResult OnPost(int categoryid)
    {
        var model = _repository.Get(categoryid);
        if (model == null) return NotFound("không tìm thấy danh mục");
        _repository.Delete(model);
        StatusMessage = $"Bạn vừa xóa danh mục: {model.Name}";
        return RedirectToPage("./Index");
    }
}
