using Clothings_Store.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clothings_Store.Areas.Admin.Pages.Role
{
    public class RolePageModel : PageModel
    {
        protected readonly RoleManager<IdentityRole> _roleManager;
        protected readonly StoreContext _context;
        [TempData]
        public string StatusMessage { get; set; }
        public RolePageModel(RoleManager<IdentityRole> roleManager, StoreContext context)
        {
            _roleManager = roleManager;
            _context = context;
        }
    }
}
