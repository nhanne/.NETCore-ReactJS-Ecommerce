//using Clothings_Store.Data;
//using Clothings_Store.Interface;
//using Clothings_Store.Models.Database;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using System.ComponentModel.DataAnnotations;

//namespace Clothings_Store.Areas.Admin.Pages.Products;

//[Authorize(Roles = "Shop Master")]
//public class EditModel : PageModel
//{
//    private readonly IRepository<Product, int> _repository;
//    private readonly IRepository<Category, int> _categoryRepository;
//    private readonly IWebHostEnvironment _environment;
//    public IEnumerable<Category> Categories { get; set; }
//    public bool Success { get; private set; } = true;
//    [TempData]
//    public string? StatusMessage { get; set; }
//    [BindProperty]
//    public Product Input { get; set; }
//    public EditModel(IRepository<Product, int> repository,
//      IRepository<Category, int> categoryRepository,
//      IWebHostEnvironment environment)
//    {
//        _repository = repository;
//        _categoryRepository = categoryRepository;
//        _environment = environment;
//    }
//    public async Task<IActionResult> OnGet(string productId)
//    {
//        Categories = await _categoryRepository.GetAllAsync();
//        if (productId == null) return NotFound("không tìm thấy sản phẩm");
//        var role = await _roleManager.FindByIdAsync(roleid);
//        if (role != null)
//        {
//            Input = new InputModel()
//            {
//                Name = role.Name
//            };
//            return Page();
//        }

//        return Page();
//    }

//    public async Task<IActionResult> OnPostAsync(string roleid)
//    {
//        if (roleid == null) return NotFound("không tìm thấy role");
//        var role = await _roleManager.FindByIdAsync(roleid);
//        if (role == null) return NotFound("không tìm thấy role");

//        if (!ModelState.IsValid)
//        {
//            return Page();
//        }
//        role.Name = Input.Name;
//        var result = await _roleManager.UpdateAsync(role);
     
//        if (result.Succeeded)
//        {
//            StatusMessage = $"Bạn vừa đổi tên role: {Input.Name}";
//            return RedirectToPage("./Index");
//        }
//        else
//        {
//            result.Errors.ToList().ForEach(error =>
//            {
//                ModelState.AddModelError(string.Empty, error.Description);
//            });
//        }
//        return Page();
//    }
//}
