﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Helpers;
using Core.Infrastructure.NotificationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MvcWeb.Areas.Admin.Models.Member;
using Services.Authentication;
using Entities.Models.Auth;

namespace MvcWeb.Areas.Admin.Controllers
{
    [Area(AreaDefaults.AdminAreaName)]
    [Authorize]
    public class MemberController : Controller
    {
        private readonly IPermissionService _permissionService;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly INotificationService _notificationService;

        public MemberController(IPermissionService permissionService,
            UserManager<User> userManager, RoleManager<Role> roleManager, INotificationService notificationService)
        {
            _permissionService = permissionService;
            _userManager = userManager;
            _roleManager = roleManager;
            _notificationService = notificationService;
        }
        public async Task<IActionResult> Roles()
        {
            var roleViewModel = new RoleListViewModel
            {
                Roles = await _roleManager.Roles.ToListAsync()
            };

            return View(roleViewModel);
        }

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

        public async Task<IActionResult> Users()
        {
            var model = new UserListViewModel
            {
                Users = await _userManager.Users.ToListAsync()
            };

            return View(model);
        }

        public async Task<IActionResult> EditUser(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return RedirectToAction(nameof(Users));

            var viewModel = new EditUserViewModel();

            var user = await _userManager.FindByIdAsync(id);

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

        public async Task<IActionResult> EditRolePermission(Guid id)
        {
            if (string.IsNullOrWhiteSpace(id.ToString())) return RedirectToAction(nameof(Roles));

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