using System;
using System.Threading.Tasks;
using AutoMapper;
using CMS.BL.Facades;
using CMS.Models.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CMS.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Policy = "Category")]
public class CategoryController(IMapper mapper, CategoryFacade categoryFacade) : Controller
{
    public async Task<IActionResult> Index()
    {
        var items = await categoryFacade.GetAll();
        return View(items);
    }

    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return NotFound();

        var item = await categoryFacade.GetById(id.Value);

        return View(item);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CategoryModel item)
    {
        if (!ModelState.IsValid) return View(item);
        await categoryFacade.Create(item);
        return RedirectToAction(nameof(Index), new { area = "Admin" });
    }

    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null) return NotFound();

        var item = await categoryFacade.GetById(id.Value);

        return View(mapper.Map<CategoryModel>(item));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, CategoryModel item)
    {
        if (id != item.Id) return NotFound();

        if (!ModelState.IsValid) return RedirectToAction(nameof(Index), new { area = "Admin" });
        try
        {
            await categoryFacade.Update(item);
        }
        catch (DbUpdateConcurrencyException)
        {
            return View(item);
        }

        return RedirectToAction(nameof(Index), new { area = "Admin" });
    }

    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null) return NotFound();

        var item = await categoryFacade.GetById(id.Value);
        return View(item);
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await categoryFacade.Remove(id);
        return RedirectToAction(nameof(Index), new { area = "Admin" });
    }
}