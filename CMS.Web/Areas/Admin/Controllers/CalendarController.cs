﻿using System;
using System.Threading.Tasks;
using AutoMapper;
using CMS.BL.Facades;
using CMS.Models.Calendar;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CalendarController : Controller
    {
        private readonly CalendarFacade _calendarFacade;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;
        
        public CalendarController(CalendarFacade calendarFacade, IWebHostEnvironment webHostEnvironment, IMapper mapper)
        {
            this._calendarFacade = calendarFacade;
            this._webHostEnvironment = webHostEnvironment;
            this._mapper = mapper;
        }
        
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var items = await _calendarFacade.GetAll();
            return View(items);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _calendarFacade.GetById(id.Value);

            return View(item);
        }

        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CalendarNewModel item)
        {
            if (ModelState.IsValid)
            {
                Guid id = await _calendarFacade.Create(item);
                return RedirectToAction(nameof(Details), new { id = id});
            }
            return View(item);
        }
        
         public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = _calendarFacade.GetById(id.Value);
            
            return View(_mapper.Map<CalendarUpdateModel>(item));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, CalendarUpdateModel item)
        {
            if (id != item.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _calendarFacade.Update(item);
                }
                catch (DbUpdateConcurrencyException)
                {
                    return View(item);
                }
                return RedirectToAction(nameof(Details), new { id = item.Id});
            }
            
            return RedirectToAction(nameof(Index));
        }
        
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _calendarFacade.GetById(id.Value);
            return View(item);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            _calendarFacade.Remove(id);
            return RedirectToAction(nameof(Index));
        }
    }
}