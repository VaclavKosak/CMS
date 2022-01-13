using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CMS.BL.Facades;
using CMS.Models.Gallery;
using CMS.Web.Models;
using CMS.Web.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "Gallery")]
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

            _targetFilePath = configuration.GetValue<string>("GalleryPath");
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
                FilesPath = GetFilesPath("")
            };
            return View(galleryView);
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
            
            var (parentUrl, urlTree) = await _galleryFacade.GetParentUrl(gallery.Id);
            ViewData["parentUrl"] = parentUrl;
            ViewData["imageFolder"] = Path.Combine(_targetFilePath, parentUrl);
            ViewData["urlTree"] = urlTree;
            
            var galleryView = new GalleryViewModel()
            {
                GalleryDetail = gallery,
                GalleryList = await _galleryFacade.GetAll(gallery.Id),
                FilesPath = GetFilesPath(parentUrl)
            };

            return View(galleryView);
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
        [Route("[action]/{parentId:guid?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GalleryNewModel gallery)
        {
            if (ModelState.IsValid)
            {
                var (parentUrl, urlTree) = await _galleryFacade.GetParentUrl(gallery.ParentId);
                var url = Path.Combine(parentUrl, gallery.Url).Replace('\\', '/');
                
                if (!CreateFolder(url))
                {
                    return View(gallery);
                }
                
                await _galleryFacade.Create(gallery);
                
                return gallery.ParentId != Guid.Empty ? 
                    RedirectToAction("Details", new { url = url, admin="Admin" }) : 
                    RedirectToAction(nameof(Index), new { admin="Admin" });
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
                var (parentUrl, urlTree) = await _galleryFacade.GetParentUrl(gallery.ParentId);
                var oldUrl = Path.Combine(parentUrl, (await _galleryFacade.GetById(id)).Url).Replace('\\', '/');
                var newUrl = Path.Combine(parentUrl, gallery.Url).Replace('\\', '/');
                
                if (!RenameFolder(oldUrl, newUrl))
                {
                    return View(gallery);
                }
                var updatedItem = await _galleryFacade.Update(gallery);

                return gallery.ParentId != Guid.Empty ? 
                    RedirectToAction("Details", new { url = newUrl, admin="Admin" }) : 
                    RedirectToAction(nameof(Index), new { admin="Admin" });
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
            var item = await _galleryFacade.GetById(id);
            var (parentUrl, urlTree) = await _galleryFacade.GetParentUrl(item.ParentId);
            var url = Path.Combine(parentUrl, item.Url).Replace('\\', '/');
            Directory.Delete(Path.Combine(_webHostEnvironment.WebRootPath, _targetFilePath, url));
            await _galleryFacade.Remove(id);
            return RedirectToAction(nameof(Index), new { area = "Admin" });
        }

        [HttpGet("RemoveFile")]
        public IActionResult RemoveFile(string filePath, string url, string fileName)
        {
            // Load all files for detele
            var file = new FileInfo(Path.Combine(_webHostEnvironment.WebRootPath, filePath, fileName));
            var fileThumbnails = new FileInfo(Path.Combine(_webHostEnvironment.WebRootPath, filePath, "thumbnails", fileName));
            var fileDetail = new FileInfo(Path.Combine(_webHostEnvironment.WebRootPath, filePath, "details", fileName));
            // Check if all files exists
            if (!file.Exists || !fileThumbnails.Exists || !fileDetail.Exists)
            {
                return NotFound();
            }
            // Delete loaded files
            file.Delete();
            fileThumbnails.Delete();
            fileDetail.Delete();
            // Redirect to back to image page
            return RedirectToAction(nameof(Details), new { url = url, area="Admin" });
        }

        [HttpGet("ReRenderImg")]
        public IActionResult ReRenderImg(string filePath, string url)
        {
            var savePath = Path.Combine(_webHostEnvironment.WebRootPath, filePath);
            
            // Remove old files
            var fileThumbnails = Path.Combine(savePath, "thumbnails");
            var fileDetails = Path.Combine(savePath, "details");

            if (string.IsNullOrEmpty(url))
            {
                url = "";
            }

            var thumbFiles = GetFilesPath(Path.Combine(url, "thumbnails"));
            var detailFiles = GetFilesPath(Path.Combine(url, "details"));
            
            RemoveFiles(thumbFiles, fileThumbnails);
            RemoveFiles(detailFiles, fileDetails);

            // Generate new files
            var files = GetFilesPath(url);
            foreach (var file in files)
            {
                // var imageProcess = new Thread(ImageHelpers.ResizeImg);
                // imageProcess.Start((savePath, file));
                ImageHelpers.ResizeImg((savePath, file));
            }

            return RedirectToAction(nameof(Details), new { url = url, area="Admin" });
        }

        private static void RemoveFiles(IEnumerable<string> files, string filePath)
        {
            foreach (var fileName in files)
            {
                var file =  new FileInfo(Path.Combine(filePath, fileName));
                if (!file.Exists)
                {
                    continue;
                }
                file.Delete();
            }
        }
        
        private string[] GetFilesPath(string url)
        {
            var saveToPath = Path.Combine(_webHostEnvironment.WebRootPath, _targetFilePath);

            url ??= "";

            saveToPath = Path.Combine(saveToPath, url);
            
            if (!Directory.Exists(saveToPath))
            {
                return Array.Empty<string>();
            } 
            
            var files = Directory.GetFiles(saveToPath)
                .Select(m => m.Remove(0, m.LastIndexOf('\\')+1)).ToArray();
            
            for (var i = 0; i < files.Length; i++)
            {
                var oldString = files[i];
                if (oldString.Contains('/'))
                {
                    files[i] = oldString[oldString.LastIndexOf("/", StringComparison.Ordinal)..].Replace("/", "");
                }
            }

            return files;
        }

        private bool CreateFolder(string url)
        {
            // Create gallery folder
            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, _targetFilePath, url);
            if (Directory.Exists(folderPath))
            {
                return false;
            }

            Directory.CreateDirectory(folderPath);

            return true;
        }

        private bool RenameFolder(string urlOld, string urlNew)
        {
            // If old url is same as new url
            if (urlOld == urlNew)
            {
                return true;
            }
            
            // Old folder path - old name
            var folderPathOld = Path.Combine(_webHostEnvironment.WebRootPath, _targetFilePath, urlOld);
            // New folder path - new name
            var folderPathNew = Path.Combine(_webHostEnvironment.WebRootPath, _targetFilePath, urlNew);
            // Check if folders exists
            if (!Directory.Exists(folderPathOld) || Directory.Exists(folderPathNew))
            {
                return false;
            }
            // Rename - move from folder to folder
            Directory.Move(folderPathOld, folderPathNew);

            return true;
        }
    }
}
