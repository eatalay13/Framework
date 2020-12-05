using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Entities.Models.Menu;

namespace Services.Authentication
{
    public interface IPermissionService
    {
        Task<bool> GetMenuItemsAsync(ClaimsPrincipal ctx, string ctrl, string act, string area = null);
        Task<List<NavigationMenu>> GetMenuItemsAsync(ClaimsPrincipal principal);
        Task<List<NavigationMenu>> GetPermissionsByRoleIdAsync(int id);
        Task<bool> SetPermissionsByRoleIdAsync(int id, IEnumerable<int> permissionIds);
    }
}
