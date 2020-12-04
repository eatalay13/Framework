using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Core.Infrastructure.PagedList;
using Entities.Models.Menu;
using Microsoft.AspNetCore.Identity;

namespace Services.Authentication
{
    public interface INavigateMenuService
    {
        IList<NavigationMenu> GetMenuList();
        IPagedList<NavigationMenu> GetMenuList(int pageIndex, int pageSize = 10);
        NavigationMenu GetMenuById(string id);
        void AddNavigationMenu(NavigationMenu menu);
        void UpdateNavigationMenu(NavigationMenu menu);
        IList<NavigationMenu> GetAllAuthorizeController(Assembly assembly);
    }
}
