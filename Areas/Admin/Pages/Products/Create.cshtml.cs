using Clothings_Store.Interface;
using Clothings_Store.Models.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Clothings_Store.Areas.Admin.Pages.Products;
[Authorize(Roles = "Shop Master")]
public class CreateModel : PageModel
{
    private readonly IRepository<Product, int> _repository;
    private readonly IRepository<Category, int> _categoryRepository;
    private readonly IWebHostEnvironment _environment;
    public IEnumerable<Category> Categories { get; set; }
    public bool Success { get; private set; } = true;
    [TempData]
    public string? StatusMessage { get; set; }
    [BindProperty]
    public Product Input { get; set; }
    public CreateModel(IRepository<Product, int> repository, 
        IRepository<Category, int> categoryRepository,
        IWebHostEnvironment environment) {
        _repository = repository;
        _categoryRepository = categoryRepository;
        _environment = environment;
    } 
    public async Task OnGet()
    {
        Categories = await _categoryRepository.GetAllAsync();
    }
    public async Task<IActionResult> OnPostAsync(IFormFile file)
    {
        ModelState.Remove("Input.Picture");
        ModelState.Remove("Input.Category");
        if (!ModelState.IsValid)
        {
            StatusMessage = $"Vui lòng nhập lại thông tin";
            Categories = await _categoryRepository.GetAllAsync();
            return Page();
        }
        try
        {
            var uploadFolder = Path.Combine(_environment.ContentRootPath, "wwwroot/images/sp");
            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }
            var existingFiles = Directory.GetFiles(uploadFolder);
            int fileCount = existingFiles.Length + 1;
            var cleanedProductName = Input.Name.ToLower().Trim().Replace(" ", "_");
            var newFileName = $"{cleanedProductName}_{fileCount}.jpg";
            var filePath = Path.Combine(uploadFolder, newFileName);
            using var fs = new FileStream(filePath, FileMode.Create);
            file.CopyTo(fs);
            Input.Picture = newFileName;
        }
        catch (Exception e)
        {
            Success = false;
            Console.WriteLine($"Lỗi khi tải lên hình ảnh: {e.Message}");
        }
        await _repository.CreateAsync(Input);
        StatusMessage = $"Bạn vừa tạo sản phẩm mới: {Input.Name}";
        return RedirectToPage("./Index");
    }
}
