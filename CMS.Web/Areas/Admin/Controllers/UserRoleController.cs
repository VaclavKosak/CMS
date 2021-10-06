using System;
using System.Threading.Tasks;
using CMS.BL.Facades;
using CMS.DAL.Entities;
using CMS.Models.UserRole;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "UserRole")]
    public class UserRoleController : Controller
    {
        private readonly UserFacade _userFacade;
        private readonly UserRoleFacade _userRoleFacade;
        private readonly RoleFacade _roleFacade;
        
        public UserRoleController(UserFacade userFacade, UserRoleFacade userRoleFacade, RoleFacade roleFacade)
        {
            _userFacade = userFacade;
            _userRoleFacade = userRoleFacade;
            _roleFacade = roleFacade;
        }
        
        public async Task<IActionResult> Index()
        {
            var items = await _userRoleFacade.GetAll();
            return View(items);
        }

        public async Task<IActionResult> Create()
        {
            ViewData["RoleId"] = new SelectList(await _roleFacade.GetAll(), "Id", "Name");
            ViewData["UserId"] = new SelectList(await _userFacade.GetAll(), "Id", "UserName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserRoleModel applicationUserRole)
        {
            if (ModelState.IsValid)
            {
                var appUserRole = await _userRoleFacade.GetById(applicationUserRole.UserId);
                if (appUserRole == null)
                {
                    await _userRoleFacade.Create(applicationUserRole);
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewData["RoleId"] = new SelectList(await _roleFacade.GetAll(), "Id", "Name", applicationUserRole.RoleId);
            ViewData["UserId"] = new SelectList(await _userFacade.GetAll(), "Id", "UserName", applicationUserRole.UserId);
            return View(applicationUserRole);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUserRole = await _userRoleFacade.GetById(id.Value);
            if (applicationUserRole == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(await _roleFacade.GetAll(), "Id", "Name", applicationUserRole.RoleId);
            ViewData["UserId"] = new SelectList(await _userFacade.GetAll(), "Id", "UserName", applicationUserRole.UserId);
            return View(applicationUserRole);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid? id, UserRoleModel applicationUserRole)
        {
            if (id != null && (id.Value != applicationUserRole.UserId))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _userRoleFacade.Remove(id.Value);
                    await _userRoleFacade.Create(applicationUserRole);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationUserRoleExists(applicationUserRole.UserId.ToString()))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(await _roleFacade.GetAll(), "Id", "Name", applicationUserRole.RoleId);
            ViewData["UserId"] = new SelectList(await _userFacade.GetAll(), "Id", "UserName", applicationUserRole.UserId);
            return View(applicationUserRole);
        }

        private bool ApplicationUserRoleExists(string id)
        {
            return _userRoleFacade.GetById(Guid.Parse(id)) != null;
        }
    }
}