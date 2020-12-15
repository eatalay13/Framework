using Core.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Authentication;
using System.Reflection;
using Services.NavigationMenu;

namespace MvcWeb.Areas.Admin.Controllers
{
    [Area(AreaDefaults.AdminAreaName)]
    [Authorize(policy: PolicyDefaults.AuthorizationPolicy)]
    public class NavigateMenuController : Controller
    {
        private readonly INavigateMenuService _navigateMenuService;

        public NavigateMenuController(INavigateMenuService navigateMenuService)
        {
            _navigateMenuService = navigateMenuService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SyncMenu()
        {
            _navigateMenuService.MenuSync(Assembly.GetExecutingAssembly());

            return RedirectToAction(nameof(Index));
        }
    }
}
