using Clothings_Store.Data;
using Clothings_Store.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Clothings_Store.Areas.Admin.Pages.User
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        public IndexModel(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public class UserAndRole : AppUser
        {
            public string RoleNames { get; set; }
        }
        public List<UserAndRole> users { get; set; }
        [TempData]
        public string StatusMessage { get; set; }

        public async Task OnGet()
        {
            users = await _userManager.Users.OrderBy(u => u.UserName).Select(u => new UserAndRole()
            {
                Id  = u.Id,
                UserName = u.UserName,
            }).ToListAsync();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                user.RoleNames = string.Join(",", roles);
            }
        }
        public void onPost() => RedirectToPage();
    }
}
