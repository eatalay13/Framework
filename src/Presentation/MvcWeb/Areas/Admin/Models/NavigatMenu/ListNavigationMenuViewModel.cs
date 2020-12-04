using Core.Infrastructure.PagedList;
using Entities.Models.Menu;
using System.Collections.Generic;

namespace MvcWeb.Areas.Admin.Models.NavigatMenu
{
    public class ListNavigationMenuViewModel
    {
        public IPagedList<NavigationMenu> NavigationMenus { get; set; }

        public IList<NavigationMenu> NavigationMenusReflection { get; set; }
    }
}
