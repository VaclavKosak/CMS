using System;
using System.Threading.Tasks;
using AutoMapper;
using CMS.BL.Facades;
using CMS.Models.Calendar;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CalendarController : Controller
    {
        private readonly CalendarFacade _calendarFacade;
        private readonly IMapper _mapper;
        
        public CalendarController(CalendarFacade calendarFacade, IMapper mapper)
        {
            _calendarFacade = calendarFacade;
            _mapper = mapper;
        }
        
        public async Task<IActionResult> Index()
        {
            var items = await _calendarFacade.GetAll();
            return View(items);
        }
        
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
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }
        
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _calendarFacade.GetById(id.Value);
            
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
                return RedirectToAction(nameof(Index));
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
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _calendarFacade.Remove(id);
            return RedirectToAction(nameof(Index));
        }
    }
}