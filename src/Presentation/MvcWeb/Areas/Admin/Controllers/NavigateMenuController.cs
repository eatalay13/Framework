using Core.CustomAttributes;
using Core.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.NavigateMenu;
using System.Reflection;

namespace MvcWeb.Areas.Admin.Controllers
{
    [Area(AreaDefaults.AdminAreaName)]
    [Authorize(policy: PolicyDefaults.AuthorizationPolicy)]
    [ParentMenu(MenuNamesDefaults.AuthorizationTopMenu)]
    public class NavigateMenuController : Controller
    {
        private readonly INavigateMenuService _navigateMenuService;

        public NavigateMenuController(INavigateMenuService navigateMenuService)
        {
            _navigateMenuService = navigateMenuService;
        }

        [MenuItem(MenuNamesDefaults.NavigateMenus)]
        public IActionResult Index()
        {
            return View();
        }

        [MenuItem(MenuNamesDefaults.NavigateMenusSync, isVisible: false)]
        public IActionResult SyncMenu()
        {
            _navigateMenuService.MenuSync(Assembly.GetExecutingAssembly());

            return RedirectToAction(nameof(Index));
        }
    }
}
