using Core.CustomAttributes;
using Core.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace MvcWeb.Areas.Admin.Controllers
{
    public class LogController : BaseAdminController
    {
        [ParentMenu(MenuNamesDefaults.AuthorizationTopMenu)]
        [MenuItem(MenuNamesDefaults.LogIndex)]
        public IActionResult Index()
        {
            return View();
        }
    }
}