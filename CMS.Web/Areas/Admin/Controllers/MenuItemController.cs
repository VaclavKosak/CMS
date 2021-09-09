using System;
using System.Threading.Tasks;
using AutoMapper;
using CMS.BL.Facades;
using CMS.Models.MenuItem;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MenuItemController : Controller
    {
        private readonly MenuItemFacade _menuItemFacade;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        
        public MenuItemController(MenuItemFacade menuItemFacade, IMapper mapper, IConfiguration configuration)
        {
            _menuItemFacade = menuItemFacade;
            _mapper = mapper;
            _configuration = configuration;
        }
        
        public async Task<IActionResult> Index()
        {
            var items = await _menuItemFacade.GetAll();
            return View(items);
        }

        public IActionResult Create()
        {
            ViewBag.Domain = _configuration["Domain"];
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MenuItemNewModel item)
        {
            ViewBag.Domain = _configuration["Domain"];
            if (ModelState.IsValid)
            {
                Guid id = await _menuItemFacade.Create(item);
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }
        
        public async Task<IActionResult> Edit(Guid? id)
        {
            ViewBag.Domain = _configuration["Domain"];
            if (id == null)
            {
                return NotFound();
            }

            var item = await _menuItemFacade.GetById(id.Value);
            
            return View(_mapper.Map<MenuItemUpdateModel>(item));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, MenuItemUpdateModel item)
        {
            ViewBag.Domain = _configuration["Domain"];
            if (id != item.Id)
            {
                return NotFound();
            }

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

            var item = await _menuItemFacade.GetById(id.Value);
            return View(item);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _menuItemFacade.Remove(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ChangeOrder(Guid firstItem, Guid secondItem)
        {
            await _menuItemFacade.ChangeOrder(firstItem, secondItem);
            
            return RedirectToAction(nameof(Index));
        }
    }
}