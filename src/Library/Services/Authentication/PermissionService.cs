using Data.UnitOfWork;
using Entities.Models.Auth;
using Entities.Models.Menu;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Services.Authentication
{
    public class PermissionService : IPermissionService
    {
        private readonly IUnitOfWork _uow;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public PermissionService(IUnitOfWork uow, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _uow = uow;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<List<NavigationMenu>> GetMenuItemsAsync(ClaimsPrincipal principal)
        {
            var isAuthenticated = principal.Identity != null && principal.Identity.IsAuthenticated;
            if (!isAuthenticated)
                return new List<NavigationMenu>();

            var roleIds = await GetUserRoleIds(principal);

            var data = await _uow.RoleMenuRepo.TableNoTracking
                .Where(e => roleIds.Contains(e.Role.Name) && e.NavigationMenu.Visible)
                .Distinct()
                .Select(e => e.NavigationMenu)
                .ToListAsync();

            return data.OrderBy(o => o.DisplayOrder).ToList();
        }

        public async Task<bool> GetMenuItemsAsync(ClaimsPrincipal ctx, string ctrl, string act, string area = null)
        {
            var result = false;
            var roleIds = await GetUserRoleIds(ctx);

            result = await _uow.RoleMenuRepo.TableNoTracking
                .Where(e => roleIds.Contains(e.Role.Name))
                .Select(e => e.NavigationMenu)
                .AnyAsync(item => item.ControllerName == ctrl && item.ActionName == act && (area == null || item.Area == area));

            return result;
        }

        public async Task<List<NavigationMenu>> GetPermissionsByRoleIdAsync(int id)
        {
            var items = await (from m in _uow.NavigationMenuRepo.TableNoTracking
                               join rm in _uow.RoleMenuRepo.TableNoTracking
                                on new { X1 = m.Id, X2 = id } equals new { X1 = rm.NavigationMenuId, X2 = rm.RoleId }
                                into rmp
                               from rm in rmp.DefaultIfEmpty()
                               select new NavigationMenu()
                               {
                                   Id = m.Id,
                                   Name = m.Name,
                                   Area = m.Area,
                                   ActionName = m.ActionName,
                                   ControllerName = m.ControllerName,
                                   IsExternal = m.IsExternal,
                                   ExternalUrl = m.ExternalUrl,
                                   DisplayOrder = m.DisplayOrder,
                                   ParentMenuId = m.ParentMenuId,
                                   Visible = m.Visible,
                                   Permitted = rm.RoleId == id
                               })
                               .AsNoTracking()
                               .ToListAsync();

            return items;
        }

        public async Task<bool> SetPermissionsByRoleIdAsync(int id, IEnumerable<int> permissionIds)
        {
            var existing = await _uow.RoleMenuRepo.Table.Where(c => c.RoleId == id).ToListAsync();

            _uow.RoleMenuRepo.Delete(existing);

            foreach (var item in permissionIds)
            {
                _uow.RoleMenuRepo.Insert(new RoleMenu
                {
                    RoleId = id,
                    NavigationMenuId = item,
                });
            }

            _uow.SaveChanges();

            return true;
        }

        private async Task<IList<string>> GetUserRoleIds(ClaimsPrincipal ctx)
        {
            var user = await _signInManager.UserManager.GetUserAsync(ctx);

            var datas = await _userManager.GetRolesAsync(user);

            return datas;
        }
    }
}
