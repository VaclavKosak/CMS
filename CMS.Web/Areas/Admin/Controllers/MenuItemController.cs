using System;
using System.Threading.Tasks;
using AutoMapper;
using CMS.BL.Facades;
using CMS.Models.MenuItem;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CMS.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Policy = "MenuItem")]
public class MenuItemController(MenuItemFacade menuItemFacade, IMapper mapper, IConfiguration configuration)
    : Controller
{
    public async Task<IActionResult> Index()
    {
        var items = await menuItemFacade.GetAll(Guid.Empty);
        return View(items);
    }

    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return NotFound();

        var item = await menuItemFacade.GetDetailDataById(id.Value);

        return View(item);
    }

    public IActionResult Create(Guid? parentId)
    {
        ViewBag.Domain = configuration["Domain"];
        var newModel = new MenuItemModel
        {
            ParentId = parentId ?? Guid.Empty
        };
        return View(newModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MenuItemModel item)
    {
        ViewBag.Domain = configuration["Domain"];
        if (ModelState.IsValid)
        {
            var id = await menuItemFacade.Create(item);

            return item.ParentId != Guid.Empty
                ? RedirectToAction(nameof(Details), new { id = item.ParentId, area = "Admin" })
                : RedirectToAction(nameof(Index), new { area = "Admin" });
        }

        return View(item);
    }

    public async Task<IActionResult> Edit(Guid? id)
    {
        ViewBag.Domain = configuration["Domain"];
        if (id == null) return NotFound();

        var item = await menuItemFacade.GetById(id.Value);

        return View(mapper.Map<MenuItemModel>(item));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, MenuItemModel item)
    {
        ViewBag.Domain = configuration["Domain"];
        if (id != item.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                await menuItemFacade.Update(item);
            }
            catch (DbUpdateConcurrencyException)
            {
                return View(item);
            }

            return RedirectToAction(nameof(Index), new { area = "Admin" });
        }

        return RedirectToAction(nameof(Index), new { area = "Admin" });
    }

    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null) return NotFound();

        var item = await menuItemFacade.GetById(id.Value);
        return View(item);
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await menuItemFacade.Remove(id);
        return RedirectToAction(nameof(Index), new { area = "Admin" });
    }

    public async Task<IActionResult> ChangeOrder(Guid firstItem, Guid secondItem)
    {
        await menuItemFacade.ChangeOrder(firstItem, secondItem);
        var item = await menuItemFacade.GetById(firstItem);

        return item.ParentId != Guid.Empty
            ? RedirectToAction("Details", new { id = item.ParentId, area = "Admin" })
            : RedirectToAction(nameof(Index), new { area = "Admin" });
    }
}