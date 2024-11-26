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
public class CategoryController : Controller
{
    private readonly CategoryFacade _categoryFacade;
    private readonly IMapper _mapper;

    public CategoryController(IMapper mapper, CategoryFacade categoryFacade)
    {
        _mapper = mapper;
        _categoryFacade = categoryFacade;
    }

    public async Task<IActionResult> Index()
    {
        var items = await _categoryFacade.GetAll();
        return View(items);
    }

    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return NotFound();

        var item = await _categoryFacade.GetById(id.Value);

        return View(item);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CategoryNewModel item)
    {
        if (ModelState.IsValid)
        {
            var id = await _categoryFacade.Create(item);
            return RedirectToAction(nameof(Index), new { area = "Admin" });
        }

        return View(item);
    }

    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null) return NotFound();

        var item = await _categoryFacade.GetById(id.Value);

        return View(_mapper.Map<CategoryUpdateModel>(item));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, CategoryUpdateModel item)
    {
        if (id != item.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                await _categoryFacade.Update(item);
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

        var item = await _categoryFacade.GetById(id.Value);
        return View(item);
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _categoryFacade.Remove(id);
        return RedirectToAction(nameof(Index), new { area = "Admin" });
    }
}