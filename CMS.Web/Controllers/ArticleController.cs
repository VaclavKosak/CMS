using System;
using System.Threading.Tasks;
using CMS.BL.Facades;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Web.Controllers
{
    public class ArticleController : Controller
    {
        private readonly ArticleFacade _articleFacade;

        public ArticleController(ArticleFacade articleFacade)
        {
            _articleFacade = articleFacade;
        }
        
        public async Task<IActionResult> Index()
        {
            return View(await _articleFacade.GetAll());
        }
        
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _articleFacade.GetById(id.Value);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }
        
        [Route("{url}")]
        public async Task<IActionResult> Details(string url)
        {
            if (url == "")
            {
                return NotFound();
            }

            var article = await _articleFacade.GetByUrl(url);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }
    }
}
