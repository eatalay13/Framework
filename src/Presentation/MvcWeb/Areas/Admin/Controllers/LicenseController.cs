using Core.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MvcWeb.Areas.Admin.Controllers
{
    [Area(AreaDefaults.AdminAreaName)]
    [AllowAnonymous]
    public class LicenseController : Controller
    {
        [Route("/InvalidLicense")]
        public IActionResult InvalidLicense()
        {
            return View();
        }
    }
}