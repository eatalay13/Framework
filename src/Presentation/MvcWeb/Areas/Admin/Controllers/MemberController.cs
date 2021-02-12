using System.Linq;
using System.Threading.Tasks;
using Core.CustomAttributes;
using Core.Helpers;
using Core.Infrastructure.NotificationService;
using Entities.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MvcWeb.Areas.Admin.Models.Member;
using Services.Authentication;

namespace MvcWeb.Areas.Admin.Controllers
{
    [Area(AreaDefaults.AdminAreaName)]
    [Authorize(PolicyDefaults.AuthorizationPolicy)]
    [ParentMenu(MenuNamesDefaults.AuthorizationTopMenu)]
    public class MemberController : Controller
    {
        [MenuItem(MenuNamesDefaults.Roles)]
        public IActionResult Roles()
        {
            return View();
        }

        [MenuItem(MenuNamesDefaults.Users, 3)]
        public IActionResult Users()
        {
            return View();
        }
    }
}