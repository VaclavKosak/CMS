using System.Threading.Tasks;
using CMS.BL.Facades;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Web.Components;

public class MenuComponent(MenuItemFacade menuItemFacade) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var menuItems = await menuItemFacade.GetAll();
        return View(menuItems);
    }
}