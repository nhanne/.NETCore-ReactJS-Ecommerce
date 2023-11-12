using Clothings_Store.Data;
using Clothings_Store.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Clothings_Store.Areas.Admin.Pages.Role
{
    public class IndexModel : RolePageModel
    {
        public IndexModel(RoleManager<IdentityRole> roleManager, StoreContext context) : base(roleManager, context)
        {

        }
        public List<IdentityRole> roles { get; set; }

        public async Task OnGet()
        {
            roles = await _roleManager.Roles.OrderBy(r => r.Name).ToListAsync();
        }
        public void onPost() => RedirectToPage();
    }
}
