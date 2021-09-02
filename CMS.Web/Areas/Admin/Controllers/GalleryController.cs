using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CMS.BL.Facades;
using CMS.Models.Gallery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("[area]/[controller]")]
    public class GalleryController : Controller
    {
        private readonly GalleryFacade _galleryFacade;

        public GalleryController(GalleryFacade galleryFacade)
        {
            _galleryFacade = galleryFacade;
        }
        
        [Route("")]
        [Route("[action]")]
        public async Task<IActionResult> Index()
        {
            return View(await _galleryFacade.GetAll(Guid.Empty));
        }
        
        [Route("{**url}")]
        public async Task<IActionResult> Details(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                url = "";
            }

            var gallery = await _galleryFacade.GetByUrl(url);
            if (gallery == null)
            {
                return NotFound();
            }

            return View(gallery);
        }
        [Route("[action]/{id?}")]
        public IActionResult Create(Guid? parentId)
        {
            var newModel = new GalleryNewModel
            {
                ParentId = parentId ?? Guid.Empty
            };

            return View(newModel);
        }
        
        [HttpPost]
        [Route("[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GalleryNewModel gallery)
        {
            if (ModelState.IsValid)
            {
                await _galleryFacade.Create(gallery);
                return RedirectToAction(nameof(Index));
            }
            return View(gallery);
        }
        [Route("[action]/{id?}")]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _galleryFacade.GetEditedById(id.Value);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }
        
        [Route("[action]/{id?}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, GalleryUpdateModel gallery)
        {
            if (id != gallery.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var updatedItem = await _galleryFacade.Update(gallery);

                return RedirectToAction(nameof(Index));
            }
            return View(gallery);
        }
        [Route("[action]/{id?}")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _galleryFacade.GetById(id.Value);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }
        
        [HttpPost, ActionName("Delete")]
        [Route("[action]/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _galleryFacade.Remove(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
