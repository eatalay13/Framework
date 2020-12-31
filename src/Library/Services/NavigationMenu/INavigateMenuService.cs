using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Core.Infrastructure.PagedList;
using DevExtreme.AspNet.Data.ResponseModel;
using DevExtreme.AspNet.Mvc;
using Entities.Dtos;

namespace Services.NavigateMenu
{
    public interface INavigateMenuService
    {
        Task<LoadResult> BindDevExp(DataSourceLoadOptions loadOptions);
        IList<Entities.Models.Menu.NavigationMenu> GetMenuList();
        IPagedList<Entities.Models.Menu.NavigationMenu> GetMenuList(int pageIndex, int pageSize = 10);
        Entities.Models.Menu.NavigationMenu GetMenuById(int id);
        void AddNavigationMenu(Entities.Models.Menu.NavigationMenu menu);
        void UpdateNavigationMenu(Entities.Models.Menu.NavigationMenu menu);
        void DeleteNavigationMenu(int id);
        void MenuSync(Assembly assembly);
        List<AuthNavigationMenuDto> GetAllAuthorizeController(Assembly assembly);
    }
}
