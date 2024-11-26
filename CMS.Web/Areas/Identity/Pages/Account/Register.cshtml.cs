using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using CMS.DAL.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using IEmailSender = CMS.Web.Services.IEmailSender;

namespace CMS.Web.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class RegisterModel(
    UserManager<AppUser> userManager,
    SignInManager<AppUser> signInManager,
    ILogger<RegisterModel> logger,
    IEmailSender emailSender,
    IConfiguration configuration)
    : PageModel
{
    [BindProperty] public InputModel Input { get; set; }

    public string ReturnUrl { get; set; }

    public IList<AuthenticationScheme> ExternalLogins { get; set; }

    public async Task OnGetAsync(string returnUrl = null)
    {
        ReturnUrl = returnUrl;
        ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
    }

    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");
        ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        if (!ModelState.IsValid) return Page();
        var user = new AppUser { UserName = Input.Email, Email = Input.Email };
        var result = await userManager.CreateAsync(user, Input.Password);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, "User");

            logger.LogInformation("User created a new account with password.");

            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                null,
                new { area = "Identity", userId = user.Id, code, returnUrl },
                host: configuration["Domain"],
                protocol: Request.Scheme);

            await emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            if (userManager.Options.SignIn.RequireConfirmedAccount)
                return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl });

            await signInManager.SignInAsync(user, false);
            return LocalRedirect(returnUrl);
        }

        foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);

        // If we got this far, something failed, redisplay form
        return Page();
    }

    public class InputModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
            MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}