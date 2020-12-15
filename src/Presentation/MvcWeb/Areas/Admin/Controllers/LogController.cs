using Core.CustomAttributes;
using Core.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MvcWeb.Areas.Admin.Controllers
{
    [Area(AreaDefaults.AdminAreaName)]
    [Authorize(policy: PolicyDefaults.AuthorizationPolicy)]
    public class LogController : Controller
    {
        [ParentMenu(MenuNamesDefaults.AuthorizationTopMenu)]
        [MenuItem(MenuNamesDefaults.LogIndex)]
        public IActionResult Index()
        {
            return View();
        }
    }
}
