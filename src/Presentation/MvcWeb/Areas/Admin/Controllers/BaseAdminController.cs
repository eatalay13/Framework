using Core.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MvcWeb.Areas.Admin.Controllers
{
    [Area(AreaDefaults.AdminAreaName)]
    [Authorize(PolicyDefaults.AuthorizationPolicy)]
    public class BaseAdminController : Controller
    {
    }
}