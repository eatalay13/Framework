using Core.Configuration;
using Core.Infrastructure.NotificationService;
using Microsoft.AspNetCore.Mvc;
using Services.Configuration;
using System.Threading.Tasks;
using AutoMapper;
using Core.CustomAttributes;
using Core.Helpers;
using Core.Infrastructure.Providers;
using Microsoft.AspNetCore.Authorization;
using MvcWeb.Areas.Admin.Models.AppSettingsViewModels;

namespace MvcWeb.Areas.Admin.Controllers
{
    [Area(AreaDefaults.AdminAreaName)]
    [Authorize(PolicyDefaults.AuthorizationPolicy)]
    public class AppSettingController : Controller
    {
        private readonly AppSettingsService _appSettingsService;
        private readonly INotificationService _notificationService;
        private readonly ILibFileProvider _fileProvider;
        private readonly IMapper _mapper;

        public AppSettingController(AppSettingsService appSettingsService,
            INotificationService notificationService,
            ILibFileProvider fileProvider,
            IMapper mapper)
        {
            _appSettingsService = appSettingsService;
            _notificationService = notificationService;
            _fileProvider = fileProvider;
            _mapper = mapper;
        }

        [MenuItem(MenuNamesDefaults.AppSetting)]
        public IActionResult Index()
        {
            var setting = _appSettingsService.AppSettings;
            return View(_mapper.Map<SettingIndexViewModel>(setting));
        }

        [HttpPost]
        public async Task<IActionResult> Index(SettingIndexViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.LogoFormFile != null)
                model.Logo = await _fileProvider.UploadFileAsync("", model.LogoFormFile);

            if (model.FaviconFormFile != null)
                model.Favicon = await _fileProvider.UploadFileAsync("", model.FaviconFormFile);

            await _appSettingsService.SaveAppSettingsAsync(_mapper.Map<AppSettings>(model));

            _notificationService.SuccessNotification("Ayarlar güncelleştirildi.");

            return View(model);
        }
    }
}
