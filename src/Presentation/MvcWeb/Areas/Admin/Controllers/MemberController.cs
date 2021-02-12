using System.Linq;
using System.Threading.Tasks;
using Core.CustomAttributes;
using Core.Helpers;
using Core.Infrastructure.NotificationService;
using Entities.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MvcWeb.Areas.Admin.Models.Member;
using Services.Authentication;

namespace MvcWeb.Areas.Admin.Controllers
{
    [ParentMenu(MenuNamesDefaults.AuthorizationTopMenu)]
    public class MemberController : BaseAdminController
    {
        private readonly ILogger<MemberController> _logger;
        private readonly INotificationService _notificationService;
        private readonly IPermissionService _permissionService;
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;

        public MemberController(IPermissionService permissionService,
            UserManager<User> userManager, RoleManager<Role> roleManager, INotificationService notificationService,
            ILogger<MemberController> logger)
        {
            _logger = logger;
            _permissionService = permissionService;
            _userManager = userManager;
            _roleManager = roleManager;
            _notificationService = notificationService;
        }

        [MenuItem(MenuNamesDefaults.Roles)]
        public IActionResult Roles()
        {
            return View();
        }

        [MenuItem(MenuNamesDefaults.Users, 3)]
        public async Task<IActionResult> Users()
        {
            var model = new UserListViewModel
            {
                Users = await _userManager.Users.ToListAsync()
            };

            return View(model);
        }

        [ParentMenu(MenuNamesDefaults.User)]
        [MenuItem(MenuNamesDefaults.UpdateUser, 4, false)]
        public async Task<IActionResult> EditUser(int id)
        {
            if (id <= 0) return RedirectToAction(nameof(Users));

            var viewModel = new EditUserViewModel();

            var user = await _userManager.FindByIdAsync(id.ToString());

            var userRoles = await _userManager.GetRolesAsync(user);

            viewModel.Email = user?.Email;
            viewModel.UserName = user?.UserName;

            var allRoles = await _roleManager.Roles.ToListAsync();

            viewModel.Roles = allRoles.Select(x => new RoleViewModel
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
    }
}