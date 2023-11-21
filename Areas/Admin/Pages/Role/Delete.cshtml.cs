using Clothings_Store.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Clothings_Store.Areas.Admin.Pages.Role
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : RolePageModel
    {
        public DeleteModel(RoleManager<IdentityRole> roleManager, StoreContext context) : base(roleManager, context)
        {
        }
        
        public IdentityRole role {  get; set; }
        public async Task<IActionResult> OnGet(string roleid)
        {
            if (roleid == null) return NotFound("không tìm thấy role");
            var role = await _roleManager.FindByIdAsync(roleid);
            if (role == null) return NotFound("không tìm thấy role");
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(string roleid)
        {
            if (roleid == null) return NotFound("không tìm thấy role");
            var role = await _roleManager.FindByIdAsync(roleid);
            if (role == null) return NotFound("không tìm thấy role");

            var result = await _roleManager.DeleteAsync(role);
         
            if (result.Succeeded)
            {
                StatusMessage = $"Bạn vừa xóa role: {role.Name}";
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
