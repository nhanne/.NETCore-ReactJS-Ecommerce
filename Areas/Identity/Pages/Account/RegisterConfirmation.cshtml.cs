// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Clothings_Store.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Clothings_Store.Areas.Identity.Pages.Shared.Components.MessagePage;

namespace Clothings_Store.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterConfirmationModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;

        public RegisterConfirmationModel(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public string Email { get; set; }
        public string UrlContinue { set; get; }

        public async Task<IActionResult> OnGetAsync(string email, string returnUrl = null)
        {
            if (email == null)
            {
                return RedirectToPage("/Index");
            }
            returnUrl = returnUrl ?? Url.Content("~/");

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound($"Không có người dùng với email: '{email}'.");
            }
            if (user.EmailConfirmed)
            {
                return ViewComponent(MessagePage.COMPONENTNAME,
                        new MessagePage.Message()
                        {
                            title = "Thông báo",
                            htmlcontent = "Tài khoản đã xác thực, chờ chuyển hướng",
                            urlredirect = (returnUrl != null) ? returnUrl : Url.Page("/Index")
                        }

                );
            }
            Email = email;
            if (returnUrl != null)
            {
                UrlContinue = Url.Page("RegisterConfirmation", new { email = Email, returnUrl = returnUrl });
            }
            else
                UrlContinue = Url.Page("RegisterConfirmation", new { email = Email });

            return Page();
        }
    }
}
