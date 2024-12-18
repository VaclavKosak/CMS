﻿using System;
using System.Threading.Tasks;
using AutoMapper;
using CMS.BL.Facades;
using CMS.Models.Calendar;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CMS.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Policy = "Calendar")]
public class CalendarController(CalendarFacade calendarFacade, IMapper mapper) : Controller
{
    public async Task<IActionResult> Index()
    {
        var items = await calendarFacade.GetAll();
        return View(items);
    }

    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return NotFound();

        var item = await calendarFacade.GetById(id.Value);

        return View(item);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CalendarModel item)
    {
        if (ModelState.IsValid)
        {
            var id = await calendarFacade.Create(item);
            return RedirectToAction(nameof(Index), new { area = "Admin" });
        }

        return View(item);
    }

    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null) return NotFound();

        var item = await calendarFacade.GetById(id.Value);

        return View(mapper.Map<CalendarModel>(item));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, CalendarModel item)
    {
        if (id != item.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                await calendarFacade.Update(item);
            }
            catch (DbUpdateConcurrencyException)
            {
                return View(item);
            }

            return RedirectToAction(nameof(Index), new { area = "Admin" });
        }

        return View(item);
    }

    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null) return NotFound();

        var item = await calendarFacade.GetById(id.Value);
        return View(item);
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await calendarFacade.Remove(id);
        return RedirectToAction(nameof(Index), new { area = "Admin" });
    }
}