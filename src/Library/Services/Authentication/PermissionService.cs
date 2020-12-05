using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Core.Extensions;
using Data.Contexts;
using Entities.Models.Menu;
using Microsoft.EntityFrameworkCore;

namespace Services.Authentication
{
    public class PermissionService : IPermissionService
    {
        private readonly AppDbContext _context;

        public PermissionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<NavigationMenu>> GetMenuItemsAsync(ClaimsPrincipal principal)
        {
            var isAuthenticated = principal.Identity != null && principal.Identity.IsAuthenticated;
            if (!isAuthenticated)
                return new List<NavigationMenu>();

            var roleIds = await GetUserRoleIds(principal);
            var data = await (from menu in _context.RoleMenu
                              where roleIds.Contains(menu.RoleId) && !menu.NavigationMenu.Visible
                              select menu)
                              .Select(m => new NavigationMenu()
                              {
                                  Id = m.NavigationMenu.Id,
                                  Name = m.NavigationMenu.Name,
                                  Area = m.NavigationMenu.Area,
                                  ActionName = m.NavigationMenu.ActionName,
                                  ControllerName = m.NavigationMenu.ControllerName,
                                  IsExternal = m.NavigationMenu.IsExternal,
                                  ExternalUrl = m.NavigationMenu.ExternalUrl,
                                  DisplayOrder = m.NavigationMenu.DisplayOrder,
                                  ParentMenuId = m.NavigationMenu.ParentMenuId,
                                  Visible = m.NavigationMenu.Visible,
                              }).Distinct().ToListAsync();

            return data;
        }

        public async Task<bool> GetMenuItemsAsync(ClaimsPrincipal ctx, string ctrl, string act, string area = null)
        {
            var result = false;
            var roleIds = await GetUserRoleIds(ctx);
            var data = await (from menu in _context.RoleMenu
                              where roleIds.Contains(menu.RoleId)
                              select menu)
                              .Select(m => m.NavigationMenu).Distinct().ToListAsync();

            foreach (var item in data)
            {
                result = (item.ControllerName == ctrl && item.ActionName == act && (area is null || item.Area == area));
                if (result)
                    break;
            }

            return result;
        }

        public async Task<List<NavigationMenu>> GetPermissionsByRoleIdAsync(int id)
        {
            var items = await (from m in _context.NavigationMenu
                               join rm in _context.RoleMenu
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
            var existing = await _context.RoleMenu.Where(x => x.RoleId == id).ToListAsync();
            _context.RemoveRange(existing);

            foreach (var item in permissionIds)
            {
                await _context.RoleMenu.AddAsync(new RoleMenu()
                {
                    RoleId = id,
                    NavigationMenuId = item,
                });
            }

            var result = await _context.SaveChangesAsync();

            return result > 0;
        }

        private async Task<List<int>> GetUserRoleIds(ClaimsPrincipal ctx)
        {
            var userId = GetUserId(ctx);
            var data = await (from role in _context.UserRoles
                              where role.UserId == userId
                              select role.RoleId).ToListAsync();

            return data;
        }

        private static int GetUserId(ClaimsPrincipal user)
        {
            var userId = ((ClaimsIdentity)user.Identity)?.FindFirst(ClaimTypes.NameIdentifier)?.Value.ToString();
            return userId.ToInt();
        }
    }
}
