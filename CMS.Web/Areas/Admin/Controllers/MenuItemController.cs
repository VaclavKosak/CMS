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
public class MenuItemController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    private readonly MenuItemFacade _menuItemFacade;

    public MenuItemController(MenuItemFacade menuItemFacade, IMapper mapper, IConfiguration configuration)
    {
        _menuItemFacade = menuItemFacade;
        _mapper = mapper;
        _configuration = configuration;
    }

    public async Task<IActionResult> Index()
    {
        var items = await _menuItemFacade.GetAll(Guid.Empty);
        return View(items);
    }

    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return NotFound();

        var item = await _menuItemFacade.GetDetailDataById(id.Value);

        return View(item);
    }

    public IActionResult Create(Guid? parentId)
    {
        ViewBag.Domain = _configuration["Domain"];
        var newModel = new MenuItemNewModel
        {
            ParentId = parentId ?? Guid.Empty
        };
        return View(newModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MenuItemNewModel item)
    {
        ViewBag.Domain = _configuration["Domain"];
        if (ModelState.IsValid)
        {
            var id = await _menuItemFacade.Create(item);

            return item.ParentId != Guid.Empty
                ? RedirectToAction(nameof(Details), new { id = item.ParentId, area = "Admin" })
                : RedirectToAction(nameof(Index), new { area = "Admin" });
        }

        return View(item);
    }

    public async Task<IActionResult> Edit(Guid? id)
    {
        ViewBag.Domain = _configuration["Domain"];
        if (id == null) return NotFound();

        var item = await _menuItemFacade.GetById(id.Value);

        return View(_mapper.Map<MenuItemUpdateModel>(item));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, MenuItemUpdateModel item)
    {
        ViewBag.Domain = _configuration["Domain"];
        if (id != item.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                await _menuItemFacade.Update(item);
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

        var item = await _menuItemFacade.GetById(id.Value);
        return View(item);
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _menuItemFacade.Remove(id);
        return RedirectToAction(nameof(Index), new { area = "Admin" });
    }

    public async Task<IActionResult> ChangeOrder(Guid firstItem, Guid secondItem)
    {
        await _menuItemFacade.ChangeOrder(firstItem, secondItem);
        var item = await _menuItemFacade.GetById(firstItem);

        return item.ParentId != Guid.Empty
            ? RedirectToAction("Details", new { id = item.ParentId, area = "Admin" })
            : RedirectToAction(nameof(Index), new { area = "Admin" });
    }
}