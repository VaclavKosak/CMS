using System.Threading.Tasks;
using CMS.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace CMS.Web.Areas.Identity.Pages.Account.Manage;

public class PersonalDataModel(
    UserManager<AppUser> userManager,
    ILogger<PersonalDataModel> logger)
    : PageModel
{
    private readonly ILogger<PersonalDataModel> _logger = logger;

    public async Task<IActionResult> OnGet()
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");

        return Page();
    }
}