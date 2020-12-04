using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Data.Contexts;
using Entities.Models.Menu;
using MvcWeb.Framework.Extensions;
using Services.Authentication;
using System.Reflection;

namespace MvcWeb.Controllers
{
    [Route("api/[controller]/[action]")]
    public class NavigationMenuApiController : Controller
    {
        private readonly AppDbContext _context;
        private readonly INavigateMenuService _navigateMenuService;

        public NavigationMenuApiController(AppDbContext context, INavigateMenuService navigateMenuService)
        {
            _navigateMenuService = navigateMenuService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions)
        {
            var result = await _navigateMenuService.BindDevExp(loadOptions);

            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAuth(DataSourceLoadOptions loadOptions)
        {
            var controllers = _navigateMenuService.GetAllAuthorizeController(Assembly.GetExecutingAssembly());

            return Json(await DataSourceLoader.LoadAsync(controllers.AsQueryable(), loadOptions));
        }

        [HttpPost]
        public IActionResult Post(string values)
        {
            var model = new NavigationMenu();

            model.PopulateModel(values);

            if (!TryValidateModel(model))
                return BadRequest(ModelState.GetFullErrorMessage());

            _navigateMenuService.AddNavigationMenu(model);

            return Json(new { model.Id });
        }

        [HttpPut]
        public IActionResult Put(Guid key, string values)
        {
            var model = _navigateMenuService.GetMenuById(key);

            if (model == null)
                return StatusCode(409, "Object not found");

            model.PopulateModel(values);

            if (!TryValidateModel(model))
                return BadRequest(ModelState.GetFullErrorMessage());

            _navigateMenuService.UpdateNavigationMenu(model);

            return Ok();
        }

        [HttpDelete]
        public async Task Delete(Guid key)
        {
            var model = await _context.NavigationMenu.FirstOrDefaultAsync(item => item.Id == key);

            _context.NavigationMenu.Remove(model);
            await _context.SaveChangesAsync();
        }


        [HttpGet]
        public async Task<IActionResult> NavigationMenuLookup(DataSourceLoadOptions loadOptions)
        {
            var lookup = from i in _context.NavigationMenu
                         orderby i.Name
                         select new
                         {
                             Value = i.Id,
                             Text = i.Name
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }
    }
}