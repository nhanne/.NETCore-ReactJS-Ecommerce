using Clothings_Store.Interface;
using Clothings_Store.Models.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clothings_Store.Areas.Admin.Pages.Categories;
[Authorize(Roles = "Shop Master")]
public class CreateModel : PageModel
{
    private readonly IRepository<Category, int> _repository;
    public CreateModel(IRepository<Category, int> repository)
    {
        _repository = repository;
    }
    [TempData]
    public string StatusMessage { get; set; }
    [BindProperty]
    public Category Input { get; set; }
    public void OnGet() { }
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();
        await _repository.CreateAsync(Input);
        StatusMessage = $"Bạn vừa tạo danh mục mới: {Input.Name}";
        return RedirectToPage("./Index");
    }
}
