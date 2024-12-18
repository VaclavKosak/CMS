using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Policy = "Home")]
[Route("[area]")]
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}