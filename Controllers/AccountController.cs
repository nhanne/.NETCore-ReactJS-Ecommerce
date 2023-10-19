using Clothings_Store.Data;
using Clothings_Store.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Clothings_Store.Controllers
{
    public class AccountController : Controller
    {
        private readonly StoreContext _db;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(StoreContext context, SignInManager<AppUser> signInManager)
        {
            _db = context;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        
    }
}
