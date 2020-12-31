using Core.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
