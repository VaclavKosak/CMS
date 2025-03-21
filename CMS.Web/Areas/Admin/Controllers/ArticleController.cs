using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CMS.BL.Facades;
using CMS.Common.Enums;
using CMS.Models.Article;
using CMS.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CMS.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Policy = "Article")]
public class ArticleController(
    IMapper mapper,
    ArticleFacade articleFacade,
    IConfiguration configuration,
    CategoryFacade categoryFacade)
    : Controller
{
    public async Task<IActionResult> Index()
    {
        var items = await articleFacade
            .GetAll();

        items = items.Where(p => p.PageType != PageType.PagePart).ToList();

        return View(items);
    }

    public async Task<IActionResult> PageParts()
    {
        var pageParts = new PagePartsViewModel
        {
            Parts = new List<PagePartsItemModel>
            {
                new()
                {
                    PartName = "ABC",
                    Articles = new List<ArticleModel>
                    {
                        //await _articleFacade.GetByUrl("abc-art"), 
                    }
                },
                new()
                {
                    PartName = "DEF",
                    Articles = new List<ArticleModel>
                    {
                        //await _articleFacade.GetByUrl("def-art"), 
                    }
                }
            }
        };

        return View(pageParts);
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Category = new SelectList(await categoryFacade.GetAll(), "Id", "Name");
        ViewBag.Domain = configuration["Domain"];
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ArticleModel item)
    {
        ViewBag.Domain = configuration["Domain"];
        if (ModelState.IsValid)
        {
            var id = await articleFacade.Create(item);
            return RedirectToAction(nameof(Index), new { area = "Admin" });
        }

        ViewBag.Category = new SelectList(await categoryFacade.GetAll(), "Id", "Name");
        return View(item);
    }

    public async Task<IActionResult> Edit(Guid? id)
    {
        ViewBag.Domain = configuration["Domain"];
        if (id == null) return NotFound();

        var item = await articleFacade.GetById(id.Value);

        var categoryListIds = item.Category.Select(categoryEntity => categoryEntity.Id).ToList();

        ViewBag.Category = new MultiSelectList(await categoryFacade.GetAll(), "Id", "Name", categoryListIds);

        return View(mapper.Map<ArticleModel>(item));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, ArticleModel item)
    {
        ViewBag.Domain = configuration["Domain"];
        if (id != item.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                await articleFacade.Update(item);
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

        var item = await articleFacade.GetById(id.Value);
        return View(item);
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await articleFacade.Remove(id);
        return RedirectToAction(nameof(Index), new { area = "Admin" });
    }
}