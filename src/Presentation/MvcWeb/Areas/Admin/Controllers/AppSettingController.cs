using Core.Configuration;
using Core.Infrastructure.NotificationService;
using Microsoft.AspNetCore.Mvc;
using Services.Configuration;
using System.Threading.Tasks;
using Core.CustomAttributes;
using Core.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace MvcWeb.Areas.Admin.Controllers
{
    [Area(AreaDefaults.AdminAreaName)]
    [Authorize(PolicyDefaults.AuthorizationPolicy)]
    public class AppSettingController : Controller
    {
        private readonly AppSettingsService _appSettingsService;
        private readonly INotificationService _notificationService;

        public AppSettingController(AppSettingsService appSettingsService, INotificationService notificationService)
        {
            _appSettingsService = appSettingsService;
            _notificationService = notificationService;
        }

        [MenuItem(MenuNamesDefaults.AppSetting)]
        public IActionResult Index()
        {
            return View(_appSettingsService.AppSettings);
        }

        [HttpPost]
        public async Task<IActionResult> Index(AppSettings appSettings)
        {
            await _appSettingsService.SaveAppSettingsAsync(appSettings);

            _notificationService.SuccessNotification("Ayarlar güncelleştirildi.");

            return View(appSettings);
        }
    }
}
