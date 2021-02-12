using Core.CustomAttributes;
using Core.Helpers;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace MvcWeb.Areas.Admin.Controllers
{
    public class HomeController : BaseAdminController
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