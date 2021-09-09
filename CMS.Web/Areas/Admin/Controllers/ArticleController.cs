using System;
using System.Threading.Tasks;
using AutoMapper;
using CMS.BL.Facades;
using CMS.Models.Article;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ArticleController : Controller
    {
        private readonly ArticleFacade _articleFacade;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        
        public ArticleController(IMapper mapper, ArticleFacade articleFacade, IConfiguration configuration)
        {
            _mapper = mapper;
            _articleFacade = articleFacade;
            _configuration = configuration;
        }
        
        public async Task<IActionResult> Index()
        {
            var items = await _articleFacade.GetAll();
            return View(items);
        }

        public IActionResult Create()
        {
            ViewBag.Domain = _configuration["Domain"];
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ArticleNewModel item)
        {
            ViewBag.Domain = _configuration["Domain"];
            if (ModelState.IsValid)
            {
                Guid id = await _articleFacade.Create(item);
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

            var item = await _articleFacade.GetById(id.Value);
            
            return View(_mapper.Map<ArticleUpdateModel>(item));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ArticleUpdateModel item)
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
                    await _articleFacade.Update(item);
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

            var item = await _articleFacade.GetById(id.Value);
            return View(item);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _articleFacade.Remove(id);
            return RedirectToAction(nameof(Index));
        }
    }
}