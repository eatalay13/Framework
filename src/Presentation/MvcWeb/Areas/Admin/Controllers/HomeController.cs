using Core.CustomAttributes;
using Core.Helpers;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MvcWeb.Areas.Admin.Controllers
{
    [Area(AreaDefaults.AdminAreaName)]
    [Authorize(PolicyDefaults.AuthorizationPolicy)]
    public class HomeController : Controller
    {
        [MenuItem(MenuNamesDefaults.AdminHomeIndex)]
        public IActionResult Index()
        {
            return View();
        }

        [MenuItem(MenuNamesDefaults.DevExpLanguage, isVisible: false)]
        public ActionResult CldrData()
        {
            return new CldrDataScriptBuilder()
                .SetCldrPath("~/wwwroot/cldr-data")
                .UseLocales("tr")
                .Build();
        }
    }
}