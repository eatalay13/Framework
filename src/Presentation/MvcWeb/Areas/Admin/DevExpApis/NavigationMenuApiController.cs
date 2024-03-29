﻿using System.Threading.Tasks;
using Core.CustomAttributes;
using Core.Helpers;
using DevExtreme.AspNet.Mvc;
using Entities.Models.Menu;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcWeb.Framework.Extensions;
using Services.NavigateMenu;

namespace MvcWeb.Areas.Admin.DevExpApis
{
    [Area(AreaDefaults.AdminAreaName)]
    [Authorize(PolicyDefaults.AuthorizationPolicy)]
    [ParentMenu(MenuNamesDefaults.ApiMenus, isVisible: false)]
    public class NavigationMenuApiController : Controller
    {
        private readonly INavigateMenuService _navigateMenuService;

        public NavigationMenuApiController(INavigateMenuService navigateMenuService)
        {
            _navigateMenuService = navigateMenuService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions)
        {
            var result = await _navigateMenuService.BindDevExp(loadOptions);

            return Json(result);
        }

        [HttpPost]
        public IActionResult Post(string values)
        {
            var model = new NavigationMenu();

            model.PopulateModel(values);

            if (!TryValidateModel(model))
                return BadRequest(ModelState.GetFullErrorMessage());

            if (model.ParentMenuId == 0)
                model.ParentMenuId = null;

            _navigateMenuService.AddNavigationMenuAsync(model);

            return Json(new { model.Id });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values)
        {
            var model = await _navigateMenuService.GetMenuByIdAsync(key);

            if (model == null)
                return StatusCode(409, "Object not found");

            model.PopulateModel(values);

            if (!TryValidateModel(model))
                return BadRequest(ModelState.GetFullErrorMessage());

            if (model.ParentMenuId == 0)
                model.ParentMenuId = null;

            await _navigateMenuService.UpdateNavigationMenuAsync(model);

            return Ok();
        }

        [HttpDelete]
        public async Task Delete(int key)
        {
            await _navigateMenuService.DeleteNavigationMenuAsync(key);
        }
    }
}