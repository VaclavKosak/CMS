using System.Threading.Tasks;
using CMS.BL.Facades;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Web.Components
{
    public class MenuComponent : ViewComponent
    {
        private readonly MenuItemFacade _menuItemFacade;
        public MenuComponent(MenuItemFacade menuItemFacade)
        {
            _menuItemFacade = menuItemFacade;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var menuItems = await _menuItemFacade.GetAll();
            return View(menuItems);
        }
    }
}