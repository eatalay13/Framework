using System.Linq;
using System.Threading.Tasks;
using Core.CustomAttributes;
using Core.Helpers;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Entities.Dtos;
using Entities.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MvcWeb.Framework.Extensions;
using Newtonsoft.Json;
using Services.Authentication;

namespace MvcWeb.Controllers
{
    [Area(AreaDefaults.AdminAreaName)]
    [Authorize(PolicyDefaults.AuthorizationPolicy)]
    [ParentMenu(MenuNamesDefaults.ApiMenus, isVisible: false)]
    public class RoleApiController : Controller
    {
        private readonly IPermissionService _permissionService;
        private readonly RoleManager<Role> _roleManager;

        public RoleApiController(RoleManager<Role> roleManager, IPermissionService permissionService)
        {
            _roleManager = roleManager;
            _permissionService = permissionService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions)
        {
            var roles = _roleManager.Roles.Select(e => new RoleDevApiDto
            {
                Id = e.Id,
                Name = e.Name,
                RoleMenuIds = e.RoleMenus.Select(c => c.NavigationMenuId).ToList()
            });

            return Json(await DataSourceLoader.LoadAsync(roles, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values)
        {
            var model = new RoleDevApiDto();

            JsonConvert.PopulateObject(values, model);

            if (!TryValidateModel(model))
                return BadRequest(ModelState.GetFullErrorMessage());

            var role = new Role
            {
                Name = model.Name
            };

            await _roleManager.CreateAsync(role);

            await _permissionService.SetPermissionsByRoleIdAsync(role.Id, model.RoleMenuIds);

            return Json(new {role.Id});
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values)
        {
            var role = await _roleManager.FindByIdAsync(key.ToString());

            if (role == null)
                return StatusCode(409, "Object not found");

            var model = new RoleDevApiDto();
            JsonConvert.PopulateObject(values, model);

            if (!TryValidateModel(model))
                return BadRequest(ModelState.GetFullErrorMessage());

            role.Name = model.Name;

            await _roleManager.UpdateAsync(role);

            await _permissionService.SetPermissionsByRoleIdAsync(role.Id, model.RoleMenuIds);

            return Ok();
        }

        [HttpDelete]
        public async Task Delete(int key)
        {
            var model = await _roleManager.FindByIdAsync(key.ToString());

            await _roleManager.DeleteAsync(model);
        }
    }
}