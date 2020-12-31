using Core.Exceptions;
using Core.Extensions;
using Core.Helpers;
using Data.Contexts;
using Entities.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MvcWeb.Models;
using Services.Authentication;
using Services.NavigateMenu;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MvcWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INavigateMenuService _navigateMenuService;
        private readonly IPermissionService _permissionService;
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _appDbContext;

        public HomeController(ILogger<HomeController> logger,
            INavigateMenuService navigateMenuService,
            IPermissionService permissionService,
            RoleManager<Role> roleManager,
            UserManager<User> userManager,
            AppDbContext appDbContext)
        {
            _logger = logger;
            _navigateMenuService = navigateMenuService;
            _permissionService = permissionService;
            _roleManager = roleManager;
            _userManager = userManager;
            _appDbContext = appDbContext;
        }

        public IActionResult Index()
        {
            var list = _navigateMenuService.GetAllAuthorizeController(Assembly.GetExecutingAssembly());
;            return View(list);
        }

        [Route("SeedData")]
        public async Task<IActionResult> SeedData()
        {
            if (!_appDbContext.Database.GetAppliedMigrations().Any())
                _appDbContext.Database.Migrate();

            _navigateMenuService.MenuSync(Assembly.GetExecutingAssembly());

            var newUser = new User
            {
                UserName = "EmrahAtalay",
                Email = "emrahatalay92@gmail.com"
            };

            await _userManager.CreateAsync(newUser, "emrah1234");

            var role = new Role { Name = "Admin" };

            await _roleManager.CreateAsync(role);

            var permission = await _permissionService.GetPermissionsByRoleIdAsync(role.Id);

            await _permissionService.SetPermissionsByRoleIdAsync(role.Id, permission.Select(x => x.Id));

            var user = await _userManager.FindByNameAsync("EmrahAtalay");

            await _userManager.AddToRoleAsync(user, role.Name);

#if DEBUG
            string sqlResName = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + "/Documents/Sql/Views/";
#else
            string sqlResName = Directory.GetCurrentDirectory() + "/Documents/Sql/Views/";
#endif
            var sqlFiles = Directory.EnumerateFiles(sqlResName, "*.sql");

            foreach (string name in sqlFiles)
            {
                string readText = System.IO.File.ReadAllText(name);
                _appDbContext.Database.ExecuteSqlRaw(readText);
            }

            return RedirectToAction("Index", "Home", new { Area = AreaDefaults.AdminAreaName });
        }

        [AllowAnonymous]
        [Route("Error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var exFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            if (exFeature == null)
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Message = "Error Occured", IsSuccess = false });


            Exception ex = exFeature.Error;
#if DEBUG
            string exMessage = string.Empty;

            if (exFeature.Error.GetType() == typeof(BusinessException))
                exMessage = ex.Message;
            else
                exMessage = ex.ToString();

            var error = new { Message = exMessage, IsSuccess = false };
#else
            var error = new { Message = ex.Message, IsSuccess = false };
#endif
            _logger.LogError(error.Message);

            if (HttpContext.Request.IsAjaxRequest())
            {
                return StatusCode(StatusCodes.Status500InternalServerError, error);
            }

            return View("Error", new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Exception = ex
            });
        }
    }
}
