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
using System.Linq;
using System.Threading.Tasks;
using Core.CustomAttributes;

namespace MvcWeb.Areas.Admin.Controllers
{
    [Area(AreaDefaults.AdminAreaName)]
    [Authorize(policy: PolicyDefaults.AuthorizationPolicy)]
    [ParentMenu(MenuNamesDefaults.AuthorizationTopMenu)]
    public class MemberController : Controller
    {
        private readonly IPermissionService _permissionService;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly INotificationService _notificationService;
        private readonly ILogger<MemberController> _logger;

        public MemberController(IPermissionService permissionService,
            UserManager<User> userManager, RoleManager<Role> roleManager, INotificationService notificationService, ILogger<MemberController> logger)
        {
            _logger = logger;
            _permissionService = permissionService;
            _userManager = userManager;
            _roleManager = roleManager;
            _notificationService = notificationService;
        }

        [ParentMenu(MenuNamesDefaults.Role)]
        [MenuItem(MenuNamesDefaults.Roles,order:1)]
        public async Task<IActionResult> Roles()
        {
            var roleViewModel = new RoleListViewModel
            {
                Roles = await _roleManager.Roles.ToListAsync()
            };

            return View(roleViewModel);
        }

        [ParentMenu(MenuNamesDefaults.Role)]
        [MenuItem(MenuNamesDefaults.CreateRole, order: 2)]
        public IActionResult CreateRole()
        {
            return View(new AddRoleViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(AddRoleViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);

            var result = await _roleManager.CreateAsync(new Role() { Name = viewModel.Name });
            if (result.Succeeded)
            {
                _notificationService.SuccessNotification("Role başarıyla eklendi.");
                return RedirectToAction(nameof(Roles));
            }
            ModelState.AddModelError("", string.Join(",", result.Errors));

            return View(viewModel);
        }

        [ParentMenu(MenuNamesDefaults.User)]
        [MenuItem(MenuNamesDefaults.Users, order: 3)]
        public async Task<IActionResult> Users()
        {
            var model = new UserListViewModel
            {
                Users = await _userManager.Users.ToListAsync()
            };

            return View(model);
        }

        [ParentMenu(MenuNamesDefaults.User)]
        [MenuItem(MenuNamesDefaults.UpdateUser, order: 4,isVisible:false)]
        public async Task<IActionResult> EditUser(int id)
        {
            if (id <= 0) return RedirectToAction(nameof(Users));

            var viewModel = new EditUserViewModel();

            var user = await _userManager.FindByIdAsync(id.ToString());

            var userRoles = await _userManager.GetRolesAsync(user);

            viewModel.Email = user?.Email;
            viewModel.UserName = user?.UserName;

            var allRoles = await _roleManager.Roles.ToListAsync();

            viewModel.Roles = allRoles.Select(x => new RoleViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Selected = userRoles.Contains(x.Name)
            }).ToArray();

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);

            var user = await _userManager.FindByIdAsync(viewModel.Id);
            var userRoles = await _userManager.GetRolesAsync(user);

            user.Email = viewModel.Email;
            user.UserName = viewModel.UserName;

            await _userManager.UpdateAsync(user);

            await _userManager.RemoveFromRolesAsync(user, userRoles);
            await _userManager.AddToRolesAsync(user, viewModel.Roles.Where(x => x.Selected).Select(x => x.Name));

            _notificationService.SuccessNotification("Müşteri başarıyla güncelleştirildi.");

            return View(viewModel);
        }

        public async Task<IActionResult> EditRolePermission(int id)
        {
            if (id <= 0) return RedirectToAction(nameof(Roles));

            var model = new NavigationMenuViewModel
            {
                Id = id,
                Permissions = await _permissionService.GetPermissionsByRoleIdAsync(id)
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRolePermission(NavigationMenuViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);

            var permissionIds = viewModel.Permissions.Where(x => x.Permitted).Select(x => x.Id);

            await _permissionService.SetPermissionsByRoleIdAsync(viewModel.Id, permissionIds);

            return RedirectToAction(nameof(Roles));
        }
    }
}
