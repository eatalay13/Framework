using System.Reflection;
using System.Threading.Tasks;
using Core.CustomAttributes;
using Core.Helpers;
using Microsoft.AspNetCore.Mvc;
using Services.NavigateMenu;

namespace MvcWeb.Areas.Admin.Controllers
{
    [ParentMenu(MenuNamesDefaults.AuthorizationTopMenu)]
    public class NavigateMenuController : BaseAdminController
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
        public async Task<IActionResult> SyncMenu()
        {
            await _navigateMenuService.MenuSyncAsync(Assembly.GetExecutingAssembly());

            return RedirectToAction(nameof(Index));
        }
    }
}