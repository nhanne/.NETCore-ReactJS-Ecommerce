using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Clothings_Store.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clothings_Store.Areas.Admin.Pages.User
{
    [Authorize(Roles = "Admin")]
    public class SetPasswordModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public SetPasswordModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [BindProperty]
        public InputModel Input { get; set; }
        [TempData]
        public string StatusMessage { get; set; }
        public class InputModel
        {
            [Required(ErrorMessage = "Phải nhập {0}")]
            [StringLength(100, ErrorMessage = "{0} phải dài {2} đến {1} ký tự.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Mật khẩu mới")]
            public string NewPassword { get; set; }
            [DataType(DataType.Password)]
            [Display(Name = "Xác nhận mật khẩu mới")]
            [Compare("NewPassword", ErrorMessage = "Mật khẩu xác nhận không trùng khớp.")]
            public string ConfirmPassword { get; set; }
        }
        public AppUser user { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound($"Không có user");
            user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Không có tài khoản id = {id}.");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound($"Không có user");
            user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Không có tài khoản id = {id}.");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }
            _userManager.RemovePasswordAsync(user);
            var addPasswordResult = await _userManager.AddPasswordAsync(user, Input.NewPassword);
            if (!addPasswordResult.Succeeded)
            {
                foreach (var error in addPasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            StatusMessage = $"Cập nhật mật khẩu cho user: {user.UserName} thành công.";

            return RedirectToPage("./Index");
        }
    }
}
