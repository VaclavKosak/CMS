using System.Threading.Tasks;
using CMS.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace CMS.Web.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class LogoutModel(SignInManager<AppUser> signInManager, ILogger<LogoutModel> logger)
    : PageModel
{
    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPost(string returnUrl = null)
    {
        await signInManager.SignOutAsync();
        logger.LogInformation("User logged out.");
        if (returnUrl != null) return LocalRedirect(returnUrl);

        return RedirectToPage();
    }
}