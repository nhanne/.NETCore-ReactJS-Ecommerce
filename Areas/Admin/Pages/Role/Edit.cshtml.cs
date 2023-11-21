using Clothings_Store.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Clothings_Store.Areas.Admin.Pages.Role
{
    [Authorize(Roles = "Admin")]
    public class EditModel : RolePageModel
    {
        public EditModel(RoleManager<IdentityRole> roleManager, StoreContext context) : base(roleManager, context)
        {
        }
        public class InputModel
        {
            [Display(Name ="Tên của role")]
            [Required(ErrorMessage = "Phải nhập {0}")]
            [StringLength(256, MinimumLength = 3, ErrorMessage = "{0} phải dài từ {2} đến {1} ký tự")]
            public string Name { get; set;}
        }
        [BindProperty]
        public InputModel Input { get; set; }   
        public IdentityRole role {  get; set; }
        public async Task<IActionResult> OnGet(string roleid)
        {
            if (roleid == null) return NotFound("không tìm thấy role");
            var role = await _roleManager.FindByIdAsync(roleid);
            if (role != null)
            {
                Input = new InputModel()
                {
                    Name = role.Name
                };
                return Page();
            }

            return Page();
        }
        public async Task<IActionResult> OnPostAsync(string roleid)
        {
            if (roleid == null) return NotFound("không tìm thấy role");
            var role = await _roleManager.FindByIdAsync(roleid);
            if (role == null) return NotFound("không tìm thấy role");

            if (!ModelState.IsValid)
            {
                return Page();
            }
            role.Name = Input.Name;
            var result = await _roleManager.UpdateAsync(role);
         
            if (result.Succeeded)
            {
                StatusMessage = $"Bạn vừa đổi tên role: {Input.Name}";
                return RedirectToPage("./Index");
            }
            else
            {
                result.Errors.ToList().ForEach(error =>
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                });
            }
            return Page();
        }
    }
}
