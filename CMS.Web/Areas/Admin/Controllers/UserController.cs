using System;
using System.Threading.Tasks;
using CMS.BL.Facades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "UserControls")]
    public class UserController : Controller
    {
        private readonly UserFacade _userFacade;
        public UserController(UserFacade userFacade)
        {
            _userFacade = userFacade;
        }
        
        public async Task<IActionResult> Index()
        {
            var users = await _userFacade.GetAll();
            return View(users);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userDetail = await _userFacade.GetById(id.Value);
            if (userDetail == null)
            {
                return NotFound();
            }
            return View(userDetail);
        }
        
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userFacade.GetById(id.Value);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _userFacade.Remove(id);
            return RedirectToAction(nameof(Index));
        }
    }
}