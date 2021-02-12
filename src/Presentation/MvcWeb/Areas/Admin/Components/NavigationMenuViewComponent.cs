using System.Threading.Tasks;
using Core.Infrastructure.TreeItem;
using Microsoft.AspNetCore.Mvc;
using Services.Authentication;

namespace MvcWeb.Areas.Admin.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private readonly IPermissionService _permissionService;

        public NavigationMenuViewComponent(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await _permissionService.GetMenuItemsAsync(HttpContext.User);

            var treeMenu = items.GenerateTree(e => e.Id, c => c.ParentMenuId);

            return View(treeMenu);
        }
    }
}