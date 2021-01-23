using Core.CustomAttributes;
using Core.Exceptions;
using Core.Helpers;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Entities.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MvcWeb.Framework.Extensions;
using System.Threading.Tasks;

namespace MvcWeb.Controllers
{
    [Area(AreaDefaults.AdminAreaName)]
    [Authorize(policy: PolicyDefaults.AuthorizationPolicy)]
    [ParentMenu(MenuNamesDefaults.ApiMenus, isVisible: false)]
    public class UsersApiController : Controller
    {
        private readonly UserManager<User> _userManager;

        public UsersApiController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions)
        {
            var data = _userManager.Users;

            var result = await DataSourceLoader.LoadAsync(data, loadOptions);

            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values)
        {
            var model = new User();

            model.PopulateModel(values);

            if (!TryValidateModel(model))
                return BadRequest(ModelState.GetFullErrorMessage());

            await _userManager.CreateAsync(model, "12345");

            return Json(new { model.Id });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values)
        {
            var user = await _userManager.FindByIdAsync(key.ToString());

            if (user == null)
                return StatusCode(409, "Object not found");

            var email = user.Email;
            var userName = user.UserName;

            user.PopulateModel(values);

            if (!TryValidateModel(user))
                return BadRequest(ModelState.GetFullErrorMessage());

            if (email != user.Email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, user.Email);
                if (!setEmailResult.Succeeded)
                {
                    throw new BusinessException($"Unexpected error occurred setting email for user with ID '{user.Id}'.");
                }
            }

            if (userName != user.UserName)
            {
                var setuserNameResult = await _userManager.SetUserNameAsync(user, user.UserName);
                if (!setuserNameResult.Succeeded)
                {
                    throw new BusinessException($"Unexpected error occurred setting email for user with ID '{user.Id}'.");
                }
            }

            await _userManager.UpdateAsync(user);

            return Ok();
        }

        [HttpDelete]
        public async Task Delete(int key)
        {
            var model = await _userManager.FindByIdAsync(key.ToString());

            await _userManager.DeleteAsync(model);
        }
    }
}