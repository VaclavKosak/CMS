using System;
using System.Threading.Tasks;
using CMS.BL.Facades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Policy = "UserControls")]
public class UserController(UserFacade userFacade) : Controller
{
    public async Task<IActionResult> Index()
    {
        var users = await userFacade.GetAll();
        return View(users);
    }

    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return NotFound();

        var userDetail = await userFacade.GetById(id.Value);
        if (userDetail == null) return NotFound();
        return View(userDetail);
    }

    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null) return NotFound();

        var user = await userFacade.GetById(id.Value);
        if (user == null) return NotFound();

        return View(user);
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await userFacade.Remove(id);
        return RedirectToAction(nameof(Index), new { area = "Admin" });
    }
}