using System.Reflection;
using Core.Helpers;
using Core.Infrastructure.NotificationService;
using Entities.Models.Menu;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcWeb.Areas.Admin.Models.NavigatMenu;
using Services.Authentication;

namespace MvcWeb.Areas.Admin.Controllers
{
    [Area(AreaDefaults.AdminAreaName)]
    [Authorize]
    public class NavigateMenuController : Controller
    {
        private readonly INavigateMenuService _navigateMenuService;
        private readonly INotificationService _notificationService;

        public NavigateMenuController(INavigateMenuService navigateMenuService,
            INotificationService notificationService)
        {
            _navigateMenuService = navigateMenuService;
            _notificationService = notificationService;
        }

        public IActionResult List(int pageIndex = 1, int pageSize = 10)
        {
            var model = new ListNavigationMenuViewModel
            {
                NavigationMenus = _navigateMenuService.GetMenuList(pageIndex, pageSize),
                NavigationMenusReflection = _navigateMenuService.GetAllAuthorizeController(Assembly.GetExecutingAssembly())
            };

            return View(model);
        }

        public IActionResult AddMenu()
        {
            var model = new AddNavigationMenuViewModel
            {
                NavigationMenus = _navigateMenuService.GetMenuList()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult AddMenu(AddNavigationMenuViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.NavigationMenus = _navigateMenuService.GetMenuList();
                return View(model);
            }

            _navigateMenuService.AddNavigationMenu(new NavigationMenu
            {
                Name = model.Name,
                ParentMenuId = model.ParentMenuId,
                Area = model.Area,
                ControllerName = model.ControllerName,
                ActionName = model.ActionName,
                IsExternal = model.IsExternal,
                ExternalUrl = model.ExternalUrl,
                DisplayOrder = model.DisplayOrder,
                Visible = model.Visible
            });

            _notificationService.SuccessNotification("Menü Başarıyla eklendi.");

            return RedirectToAction(nameof(List));
        }

        public IActionResult UpdateMenu(string id)
        {
            var menu = _navigateMenuService.GetMenuById(id);

            if (menu is null)
                return RedirectToAction(nameof(List));

            var model = new UpdateNavigationMenuViewModel
            {
                NavigationMenus = _navigateMenuService.GetMenuList(),
                Name = menu.Name,
                ParentMenuId = menu.ParentMenuId,
                Area = menu.Area,
                ControllerName = menu.ControllerName,
                ActionName = menu.ActionName,
                IsExternal = menu.IsExternal,
                ExternalUrl = menu.ExternalUrl,
                DisplayOrder = menu.DisplayOrder,
                Visible = menu.Visible,
                Permitted = menu.Permitted
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult UpdateMenu(UpdateNavigationMenuViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var menu = _navigateMenuService.GetMenuById(model.Id.ToString());

            menu.Name = model.Name;
            menu.ParentMenuId = model.ParentMenuId;
            menu.Area = model.Area;
            menu.ControllerName = model.ControllerName;
            menu.ActionName = model.ActionName;
            menu.IsExternal = model.IsExternal;
            menu.ExternalUrl = model.ExternalUrl;
            menu.DisplayOrder = model.DisplayOrder;
            menu.Visible = model.Visible;
            menu.Permitted = model.Permitted;

            _navigateMenuService.UpdateNavigationMenu(menu);

            _notificationService.SuccessNotification("Menü Başarıyla Güncellendi.");

            return RedirectToAction(nameof(List));
        }
    }
}
