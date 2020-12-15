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
        Task<List<Entities.Models.Menu.NavigationMenu>> GetMenuItemsAsync(ClaimsPrincipal principal);
        Task<List<Entities.Models.Menu.NavigationMenu>> GetPermissionsByRoleIdAsync(int id);
        Task<bool> SetPermissionsByRoleIdAsync(int id, IEnumerable<int> permissionIds);
    }
}
