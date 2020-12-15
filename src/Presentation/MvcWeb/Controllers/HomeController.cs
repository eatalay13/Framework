using Entities.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcWeb.Models;
using Services.Authentication;
using Services.NavigationMenu;
using System.Diagnostics;
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

        public HomeController(ILogger<HomeController> logger,
            INavigateMenuService navigateMenuService,
            IPermissionService permissionService,
            RoleManager<Role> roleManager,
            UserManager<User> userManager)
        {
            _logger = logger;
            _navigateMenuService = navigateMenuService;
            _permissionService = permissionService;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var list = _navigateMenuService.GetAllAuthorizeController(Assembly.GetExecutingAssembly());
;            return View(list);
        }

        public async Task<IActionResult> Privacy()
        {
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

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
