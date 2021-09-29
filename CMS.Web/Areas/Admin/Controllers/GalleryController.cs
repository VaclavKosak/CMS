using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CMS.BL.Facades;
using CMS.Models.Gallery;
using CMS.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("[area]/[controller]")]
    public class GalleryController : Controller
    {
        private readonly GalleryFacade _galleryFacade;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _targetFilePath;

        public GalleryController(GalleryFacade galleryFacade, IWebHostEnvironment webHostEnvironment,
            IConfiguration configuration)
        {
            _galleryFacade = galleryFacade;
            _webHostEnvironment = webHostEnvironment;

            _targetFilePath = configuration.GetValue<string>("StoredFilesPath");
        }
        
        [Route("")]
        [Route("[action]")]
        public async Task<IActionResult> Index()
        {
            ViewData["parentUrl"] = "";
            ViewData["imageFolder"] = _targetFilePath;
            var galleryView = new GalleryViewModel()
            {
                GalleryList = await _galleryFacade.GetAll(Guid.Empty),
                FilesPath = await GetFilesPath("")
            };
            return View(galleryView);
        }
        
        [Route("{**url}")]
        public async Task<IActionResult> Details(string url)
        {
            ViewData["parentUrl"] = url;
            
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
        [Route("[action]/{parentId:guid?}")]
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
        
        private async Task<string[]> GetFilesPath(string url)
        {
            var saveToPath = Path.Combine(_webHostEnvironment.WebRootPath, _targetFilePath);

            url ??= "";

            saveToPath = Path.Combine(saveToPath, url);
            var files = Directory.GetFiles(saveToPath)
                .Select(m => m.Remove(0, m.LastIndexOf('\\')+1)).ToArray();
                // .Select(fileName => Path.Combine(_targetFilePath, url, fileName)).ToArray();

            return files;
        }
    }
}
