using Clothings_Store.Data;
using Clothings_Store.Interface;
using Clothings_Store.Models.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NuGet.Protocol.Core.Types;
using System.ComponentModel.DataAnnotations;

namespace Clothings_Store.Areas.Admin.Pages.Categories
{
    [Authorize(Roles = "Shop Master")]
    public class CreateModel : PageModel
    {
        private readonly IRepository<Category> _repository;
        public CreateModel(IRepository<Category> repository)
        {
            _repository = repository;
        }
        [TempData]
        public string StatusMessage { get; set; }
        [BindProperty]
        public Category Input { get; set; }

        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page(); 
            }
            _repository.Add(Input);
            StatusMessage = $"Bạn vừa tạo danh mục mới: {Input.Name}";
            return RedirectToPage("./Index");
        }
    }
}
