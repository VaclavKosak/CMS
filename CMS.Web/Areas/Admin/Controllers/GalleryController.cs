using System;
using System.Threading.Tasks;
using CMS.BL.Facades;
using CMS.Models.Gallery;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GalleryController : Controller
    {
        private readonly GalleryFacade _galleryFacade;

        public GalleryController(GalleryFacade galleryFacade)
        {
            _galleryFacade = galleryFacade;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _galleryFacade.GetAll(Guid.Empty));
        }
        
        public async Task<IActionResult> Details(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return NotFound();
            }

            var gallery = await _galleryFacade.GetByUrl(url);
            if (gallery == null)
            {
                return NotFound();
            }

            return View(gallery);
        }
        
        public IActionResult Create(Guid? parentId)
        {
            var newModel = new GalleryNewModel
            {
                ParentId = parentId ?? Guid.Empty
            };

            return View(newModel);
        }
        
        [HttpPost]
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _galleryFacade.Remove(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
