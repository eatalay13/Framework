using System;
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
